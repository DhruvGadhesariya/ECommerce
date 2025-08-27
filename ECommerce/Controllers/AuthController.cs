using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.Authentication;
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

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "Invalid request",
                    Data = ModelState
                });
            }

            var id = _authService.Register(req);

            if (id is null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    Message = "Email already exists"
                });
            }

            return Ok(new ResponseModel
            {
                Id = id,
                StatusCode = 200,
                Message = "User registered successfully"
            });
        }

        /// <summary>
        /// Authenticates user and returns JWT token + claims.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var token = _authService.Login(req);

            if (token is null)
            {
                return Unauthorized(new AuthResponse
                {
                    StatusCode = 401,
                    Message = "Invalid credentials"
                });
            }

            return Ok(new AuthResponse
            {
                StatusCode = 200,
                Message = "Login successful",
                Token = token.Token,
                ExpiresAt = token.ExpiresAt,
                UserId = token.UserId,
                FullName = token.FullName,
                Role = token.Role
            });
        }

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

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Message = "User info retrieved",
                Data = new
                {
                    UserId = userId,
                    FullName = fullName,
                    Role = role
                }
            });
        }
    }
}