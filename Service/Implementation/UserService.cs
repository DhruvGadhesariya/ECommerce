using Common.Models;
using Common.Helpers;
using Data.Data;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Models.User;
using static Common.Models.PaginationModel;

namespace Service.Implementation
{
    /// <summary>
    /// Provides CRUD, pagination, caching, and file handling logic for users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly EcommercedbContext _db;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly IFileService _files;
        private readonly ILogger<UserService> _logger;

        public UserService(
            EcommercedbContext db,
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
        public List<UserModel> GetUserDetails()
        {
            if (_cache.TryGetValue(CacheKeys.UsersAll, out List<UserModel> cached))
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
        // Paged, Filtered & Sorted Users (cached 3 min)
        // ---------------------------
        public PagedResult<UserModel>? GetUsersPaged(PagedRequest q)
        {
            var key = CacheKeys.UsersPaged(q.Page, q.Size, q.Search, q.SortBy, q.Desc);
            if (_cache.TryGetValue(key, out PagedResult<UserModel> cached))
            {
                _logger.LogInformation("Returned paged users from cache {Key}", key);
                return cached;
            }

            var data = _db.Users.AsNoTracking().Where(x => x.DeletedAt == null);

            // 🔍 Filtering
            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.Trim().ToLower();
                data = data.Where(x =>
                    x.Email.ToLower().Contains(s) ||
                    x.Firstname.ToLower().Contains(s) ||
                    x.Lastname.ToLower().Contains(s));
            }

            // ↕ Sorting
            if (!string.IsNullOrWhiteSpace(q.SortBy))
            {
                bool desc = q.Desc;
                data = q.SortBy.ToLower() switch
                {
                    "firstname" => desc ? data.OrderByDescending(x => x.Firstname) : data.OrderBy(x => x.Firstname),
                    "lastname" => desc ? data.OrderByDescending(x => x.Lastname) : data.OrderBy(x => x.Lastname),
                    "email" => desc ? data.OrderByDescending(x => x.Email) : data.OrderBy(x => x.Email),
                    "createdat" => desc ? data.OrderByDescending(x => x.CreatedAt) : data.OrderBy(x => x.CreatedAt),
                    _ => data.OrderBy(x => x.UserId) // default sort
                };
            }

            var total = data.LongCount();
            var items = data
                .Skip((q.Page - 1) * q.Size)
                .Take(q.Size)
                .Select(ToUserModel)
                .ToList();

            var result = new PagedResult<UserModel>
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
        public long? AddUser(AddUserModel model)
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

            _cache.Remove(CacheKeys.UsersAll); // invalidate
            _logger.LogInformation("Added new user {UserId} and invalidated cache", entity.UserId);

            return entity.UserId;
        }

        // ---------------------------
        // Update User
        // ---------------------------
        public ResponseModel UpdateUser(long userId, UpdateUserModel model)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == userId && x.DeletedAt == null);
            if (user == null)
                return new ResponseModel { Id = userId, StatusCode = 404, Message = "User Doesn't Exist!" };

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase) &&
                _db.Users.Any(x => x.Email == model.Email && x.DeletedAt == null))
            {
                return new ResponseModel { StatusCode = 401, Message = "User Already Exist!" };
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
            return new ResponseModel { Id = userId, StatusCode = 200, Message = "Updated" };
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

            // delete old avatar
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
        public UserModel? GetUserDetailById(long userId)
        {
            return _db.Users
                .Where(x => x.UserId == userId && x.DeletedAt == null)
                .Select(ToUserModel)
                .FirstOrDefault();
        }

        // ---------------------------
        // Helper: Map entity -> model
        // ---------------------------
        private static UserModel ToUserModel(User x) => new UserModel
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
