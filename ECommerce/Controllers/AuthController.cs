using Common.Dtos;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Dtos.Authentication;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    /// <summary>
    /// Handles authentication-related operations:
    /// User registration, login, and profile info retrieval.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Registration

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto
                {
                    StatusCode = 400,
                    Message = Messages.InvalidRequest,
                    Data = ModelState
                });
            }

            var id = _authService.Register(req);

            if (id is null)
            {
                return Conflict(new ApiResponseDto
                {
                    StatusCode = 409,
                    Message = Messages.EmailAlreadyExists
                });
            }

            return Ok(new ApiResponseDto
            {
                Id = id,
                StatusCode = 200,
                Message = Messages.UserRegistered
            });
        }

        #endregion

        #region Login

        /// <summary>
        /// Authenticates user and returns JWT token + claims.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto req)
        {
            var token = _authService.Login(req);

            if (token is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    StatusCode = 401,
                    Message = Messages.InvalidCredentials
                });
            }

            return Ok(new AuthResponseDto
            {
                StatusCode = 200,
                Message = Messages.LoginSuccessful,
                Token = token.Token,
                ExpiresAt = token.ExpiresAt,
                UserId = token.UserId,
                FullName = token.FullName,
                Role = token.Role
            });
        }

        #endregion

        #region User Info

        /// <summary>
        /// Gets information about the currently logged-in user from JWT claims.
        /// </summary>
        [Authorize]
        [HttpGet("myinfo")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new ApiResponseDto
            {
                StatusCode = 200,
                Message = Messages.UserInfoRetrieved,
                Data = new
                {
                    UserId = userId,
                    FullName = fullName,
                    Role = role
                }
            });
        }
        #endregion
    }
}
