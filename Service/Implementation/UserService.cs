using Common.Dtos;
using Common.Helpers;
using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Dtos.User;
using static Common.Dtos.PaginationParamsDto;

namespace Service.Implementation
{
    /// <summary>
    /// Provides CRUD, pagination, caching, and file handling logic for users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserHubDbContext _db;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly IFileService _files;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserHubDbContext db,
            IMemoryCache cache,
            IConfiguration config,
            IFileService files,
            ILogger<UserService> logger)
        {
            _db = db;
            _cache = cache;
            _config = config;
            _files = files;
            _logger = logger;
        }

        /// <summary>
        /// Folder path for avatar uploads (from config, fallback = "uploads/avatars").
        /// </summary>
        private string AvatarsFolder => _config["Upload:Avatars"] ?? "uploads/avatars";

        // ---------------------------
        // Get All Users (cached 5 min)
        // ---------------------------
        public List<UserResponseDto> GetUserDetails()
        {
            if (_cache.TryGetValue(CacheKeys.UsersAll, out List<UserResponseDto> cached))
            {
                _logger.LogInformation("Returned {Count} users from cache", cached.Count);
                return cached;
            }

            var users = _db.Users
                .Where(x => x.DeletedAt == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(ToUserModel)
                .ToList();

            _cache.Set(CacheKeys.UsersAll, users, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Cached {Count} users", users.Count);

            return users;
        }

        // ---------------------------
        // Paged, Filtered & Sorted Users (cached 5 min)
        // ---------------------------
        public PagedResult<UserResponseDto>? GetAllUsers(PagedRequest q)
        {
            var key = CacheKeys.UsersPaged(q.Page, q.Size, q.Search, q.SortBy, q.Desc);
            if (_cache.TryGetValue(key, out PagedResult<UserResponseDto> cached))
            {
                _logger.LogInformation("Returned paged users from cache {Key}", key);
                return cached;
            }

            var data = _db.Users.AsNoTracking().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.Trim().ToLower();
                data = data.Where(x =>
                    x.Email.ToLower().Contains(s) ||
                    x.Firstname.ToLower().Contains(s) ||
                    x.Lastname.ToLower().Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(q.SortBy))
            {
                bool desc = q.Desc;
                data = q.SortBy.ToLower() switch
                {
                    "firstname" => desc ? data.OrderByDescending(x => x.Firstname) : data.OrderBy(x => x.Firstname),
                    "lastname" => desc ? data.OrderByDescending(x => x.Lastname) : data.OrderBy(x => x.Lastname),
                    "email" => desc ? data.OrderByDescending(x => x.Email) : data.OrderBy(x => x.Email),
                    "createdat" => desc ? data.OrderByDescending(x => x.CreatedAt) : data.OrderBy(x => x.CreatedAt),
                    _ => data.OrderBy(x => x.UserId)
                };
            }

            var total = data.LongCount();
            var items = data
                .Skip((q.Page - 1) * q.Size)
                .Take(q.Size)
                .Select(ToUserModel)
                .ToList();

            var result = new PagedResult<UserResponseDto>
            {
                Page = q.Page,
                Size = q.Size,
                Total = total,
                Items = items
            };

            _cache.Set(key, result, TimeSpan.FromMinutes(3));
            _logger.LogInformation("Cached paged users {Key} (items: {Count})", key, items.Count);

            return result;
        }

        // ---------------------------
        // Add User
        // ---------------------------
        public long? AddUser(AddUserRequestDto model)
        {
            if (_db.Users.Any(u => u.Email == model.Email && u.DeletedAt == null))
                return null;

            var entity = new User
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Role = model.Role ?? 1,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(entity);
            _db.SaveChanges();

            _cache.Remove(CacheKeys.UsersAll);
            _logger.LogInformation("Added new user {UserId} and invalidated cache", entity.UserId);

            return entity.UserId;
        }

        // ---------------------------
        // Update User
        // ---------------------------
        public ApiResponseDto UpdateUser(long userId, UpdateUserRequestDto model)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == userId && x.DeletedAt == null);
            if (user == null)
                return new ApiResponseDto { Id = userId, StatusCode = 404, Message = "User Doesn't Exist!" };

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase) &&
                _db.Users.Any(x => x.Email == model.Email && x.DeletedAt == null))
            {
                return new ApiResponseDto { StatusCode = 401, Message = "User Already Exist!" };
            }

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Email = model.Email.ToLower();
            user.PhoneNumber = model.PhoneNumber;
            user.Role = model.Role ?? user.Role;
            user.Status = model.Status ?? user.Status;
            user.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();
            _cache.Remove(CacheKeys.UsersAll);

            _logger.LogInformation("Updated user {UserId}", userId);
            return new ApiResponseDto { Id = userId, StatusCode = 200, Message = "Updated" };
        }

        // ---------------------------
        // Soft Delete User
        // ---------------------------
        public bool DeleteUser(long userId)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == userId && x.DeletedAt == null);
            if (user == null) return false;

            user.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            _cache.Remove(CacheKeys.UsersAll);
            _logger.LogInformation("Deleted user {UserId}", userId);

            return true;
        }

        // ---------------------------
        // Update Avatar
        // ---------------------------
        public string? UpdateAvatar(long userId, IFormFile file)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == userId && x.DeletedAt == null);
            if (user == null) return null;

            var rel = _files.SaveAvatar(file, AvatarsFolder);

            if (!string.IsNullOrWhiteSpace(user.Avatar))
            {
                try { _files.DeleteFile(user.Avatar); }
                catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete old avatar {Old}", user.Avatar); }
            }

            user.Avatar = rel;
            user.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();

            _cache.Remove(CacheKeys.UsersAll);
            _logger.LogInformation("Updated avatar for user {UserId}", userId);

            return rel;
        }

        // ---------------------------
        // Get User By Id
        // ---------------------------
        public UserResponseDto? GetUserDetailById(long userId)
        {
            return _db.Users
                .Where(x => x.UserId == userId && x.DeletedAt == null)
                .Select(ToUserModel)
                .FirstOrDefault();
        }

        // ---------------------------
        // Helper: Map entity -> model
        // ---------------------------
        private static UserResponseDto ToUserModel(User x) => new UserResponseDto
        {
            UserId = x.UserId,
            Firstname = x.Firstname,
            Lastname = x.Lastname,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Role = x.Role,
            Status = x.Status,
            AvatarUrl = x.Avatar,
            CreatedAt = x.CreatedAt
        };
    }
}
