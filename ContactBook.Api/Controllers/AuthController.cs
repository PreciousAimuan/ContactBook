using ContactBook.Auth;
using ContactBook.Core.DTOs;
using ContactBook.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SQ20.Net_Week_8_9_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ITokenGenerator _tokenGenerator;
        public AuthController(UserManager<AppUser> userManager, IConfiguration config, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _config = config;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var Password = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!Password)
            {
                return BadRequest("Invalid credentials");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var UserRoles = roles.ToArray();

            //var token = _tokenGenerator.GenerateTokenAsync(user);

            var token = _tokenGenerator.GenerateToken(model.Email, user.Id, model.Password, _config, UserRoles);
            return Ok(token);
        }
    }

}
