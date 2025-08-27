using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.Authentication;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/login")]
        public IActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                AuthModel response = _authService.Login(model);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("/registration")]
        public IActionResult Registration([FromForm]RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                string user = _authService.Registration(model);
                if (user != null)
                {
                    return Ok("You Have Successfully Registered Yourself.You Can Login Now.");
                }
                else
                {
                    return BadRequest("You Are Already Registered,Please Login Directly.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("/login/{Id}")]
        public IActionResult UserDetail(long Id)
        {
            var UserDetail = _authService.GetUserDetail(Id);
            if (UserDetail != null)
            {
                return Ok(UserDetail);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("/passwordencrypt")]
        public IActionResult PasswordEncrypt(string? password)
        {
            return Ok(_authService.EncryptedPassword(password));
        }

        [Authorize(Roles = "User,Supplier,Admin")]
        [HttpGet("/authchecker")]
        public IActionResult AuthChecker()
        {
            return Ok("You Are Authorized person");
        }

        
    }
}
