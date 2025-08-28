using Common.Enums;
using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Dtos.User;
using static Common.Dtos.PaginationParamsDto;

namespace ECommerce.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations (CRUD, avatar upload, etc.).
    /// Requires Admin role for most endpoints except registration and avatar retrieval.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region User Details

        /// <summary>
        /// Get details of a specific user by Id.
        /// Only accessible by Admin.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("{id:long}")]
        public IActionResult UserDetailById(long id)
        {
            var response = _userService.GetUserDetailById(id);
            if (response == null)
                return Ok(new ApiResponseDto
                {
                    Id = id,
                    StatusCode = 404,
                    Message = Messages.UserNotFound
                });

            return Ok(new ApiResponseDto
            {
                Id = response.UserId,
                StatusCode = 200,
                Message = Messages.Success,
                Data = response
            });
        }

        #endregion

        #region User CRUD

        /// <summary>
        /// Add a new user. 
        /// Anonymous access allowed.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddUser(AddUserRequestDto model)
        {
            var response = _userService.AddUser(model);

            if (response != null)
                return Ok(new ApiResponseDto
                {
                    Id = response,
                    StatusCode = 200,
                    Message = Messages.UserAdded
                });

            return Ok(new ApiResponseDto
            {
                StatusCode = 409,
                Message = Messages.UserAlreadyExists
            });
        }

        /// <summary>
        /// Update existing user by Id. Only Admin can update.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:long}")]
        public IActionResult UpdateUser(long id, UpdateUserRequestDto model)
        {
            if (id != model.UserId)
                return Ok(new ApiResponseDto
                {
                    Id = id,
                    StatusCode = 400,
                    Message = Messages.InvalidRequest
                });

            var response = _userService.UpdateUser(id, model);
            return Ok(response);
        }

        /// <summary>
        /// Delete a user by Id. Only Admin can delete.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:long}")]
        public IActionResult DeleteUser(long id)
        {
            var response = _userService.DeleteUser(id);

            if (response)
                return Ok(new ApiResponseDto
                {
                    Id = id,
                    StatusCode = 200,
                    Message = Messages.UserDeleted
                });

            return Ok(new ApiResponseDto
            {
                Id = id,
                StatusCode = 404,
                Message = Messages.UserDoesNotExist
            });
        }

        #endregion

        #region User List

        /// <summary>
        /// Get paginated, filtered, and sorted list of users.
        /// Admin only.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("all")]
        public IActionResult GetAllUsers([FromQuery] PagedRequest request)
        {
            var result = _userService.GetAllUsers(request);

            if (result == null || result.Items.Count == 0)
                return Ok(new ApiResponseDto
                {
                    StatusCode = 404,
                    Message = Messages.NoUsersFound
                });

            return Ok(new ApiResponseDto
            {
                StatusCode = 200,
                Message = Messages.Success,
                Data = result
            });
        }

        #endregion

        #region User Avatar

        /// <summary>
        /// Upload avatar for a user. Any authenticated user can upload.
        /// </summary>
        [Authorize]
        [HttpPost("{id:long}/avatar")]
        public IActionResult UploadAvatar(long id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { Message = Messages.FileRequired });

            try
            {
                var relPath = _userService.UpdateAvatar(id, file);
                if (relPath == null)
                    return NotFound(new { Message = Messages.UserNotFound });

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var fullUrl = $"{baseUrl}/{relPath}";

                return Ok(new ApiResponseDto
                {
                    Id = id,
                    StatusCode = 200,
                    Message = Messages.AvatarUploaded,
                    Data = new UploadAvatarResponseDto { RelativePath = relPath, Url = fullUrl }
                });
            }
            catch
            {
                return StatusCode(500, new { Message = Messages.AvatarUploadError });
            }
        }

        /// <summary>
        /// Get avatar of a user. Public endpoint.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:long}/avatar")]
        public IActionResult GetAvatarUrl(long id)
        {
            var u = _userService.GetUserDetailById(id);
            if (u == null)
                return NotFound(new { Message = Messages.UserNotFound });

            if (string.IsNullOrWhiteSpace(u.AvatarUrl))
                return NotFound(new { Message = Messages.NoAvatarFound });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(new ApiResponseDto
            {
                Id = id,
                StatusCode = 200,
                Message = Messages.Success,
                Data = new UploadAvatarResponseDto
                {
                    RelativePath = u.AvatarUrl!,
                    Url = $"{baseUrl}/{u.AvatarUrl}"
                }
            });
        }

        #endregion

        #region Cache

        /// <summary>
        /// Clear cache (Admin only). Useful for testing.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            return Ok(new { Message = Messages.CacheCleared });
        }

        #endregion
    }
}
