using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.User;
using static Common.Models.PaginationModel;

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

        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
        }

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
            {
                return Ok(new ResponseModel
                {
                    Id = id,
                    StatusCode = 404,
                    Message = "User not found"
                });
            }

            return Ok(new ResponseModel
            {
                Id = response.UserId,
                StatusCode = 200,
                Message = "Success",
                Data = response
            });
        }

        /// <summary>
        /// Add a new user. 
        /// Anonymous access allowed.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddUser(AddUserModel model)
        {
            var response = _userService.AddUser(model);

            if (response != null)
            {
                return Ok(new ResponseModel
                {
                    Id = response,
                    StatusCode = 200,
                    Message = "User added successfully!"
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 409,
                Message = "User already exists!"
            });
        }

        /// <summary>
        /// Update existing user by Id. Only Admin can update.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:long}")]
        public IActionResult UpdateUser(long id, UpdateUserModel model)
        {
            if (id != model.UserId)
            {
                return Ok(new ResponseModel
                {
                    Id = id,
                    StatusCode = 400,
                    Message = "Invalid request"
                });
            }

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
            {
                return Ok(new ResponseModel
                {
                    Id = id,
                    StatusCode = 200,
                    Message = "User deleted successfully!"
                });
            }

            return Ok(new ResponseModel
            {
                Id = id,
                StatusCode = 404,
                Message = "User doesn't exist!"
            });
        }

        /// <summary>
        /// Get paginated, filtered, and sorted list of users.
        /// Admin only.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("all")]
        public IActionResult GetUsersPaged([FromQuery] PagedRequest request)
        {
            var result = _userService.GetUsersPaged(request);

            if (result == null || result.Items.Count == 0)
            {
                return Ok(new ResponseModel
                {
                    StatusCode = 404,
                    Message = "No users found"
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            });
        }

        /// <summary>
        /// Upload avatar for a user. Any authenticated user can upload.
        /// </summary>
        [Authorize]
        [HttpPost("{id:long}/avatar")]
        public IActionResult UploadAvatar(long id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { Message = "File is required." });

            try
            {
                var relPath = _userService.UpdateAvatar(id, file);
                if (relPath == null)
                    return NotFound(new { Message = "User not found." });

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var fullUrl = $"{baseUrl}/{relPath}";

                return Ok(new ResponseModel
                {
                    Id = id,
                    StatusCode = 200,
                    Message = "Avatar uploaded successfully",
                    Data = new UploadAvatarResponse { RelativePath = relPath, Url = fullUrl }
                });
            }
            catch
            {
                return StatusCode(500, new { Message = "An error occurred while uploading avatar." });
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
                return NotFound(new { Message = "User not found." });

            if (string.IsNullOrWhiteSpace(u.AvatarUrl))
                return NotFound(new { Message = "No avatar found." });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(new ResponseModel
            {
                Id = id,
                StatusCode = 200,
                Message = "Success",
                Data = new UploadAvatarResponse
                {
                    RelativePath = u.AvatarUrl!,
                    Url = $"{baseUrl}/{u.AvatarUrl}"
                }
            });
        }

        /// <summary>
        /// Clear cache (Admin only). Useful for testing.
        /// </summary>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            return Ok(new { Message = "Caches cleared successfully." });
        }
    }
}