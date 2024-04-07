using ContactBook.Core;
using ContactBook.Core.DTOs;
using ContactBook.Infrastructure;
using ContactBook.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SQ20.Net_Week_8_9_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserController(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }


        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] AppUserDTO appUser)
        {
            var newUser = await _appUserRepository.AddAppUser(appUser);
            return Ok(newUser);
        }

        [HttpGet("search-for-users")]
        /*[Authorize(Roles = "Admin")]*/
        public async Task<ActionResult<List<AppUser>>> SearchForUser([FromQuery] string term)
        {
            var AllUsers = await _appUserRepository.GetAllAsync(term);
            return Ok(AllUsers);
        }


        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var appUser = await _appUserRepository.GetAppUserById(id);
            return Ok(appUser);
        }

        [HttpGet("get-all-users")]
/*         public async Task<ActionResult<IList<AppUserDTO>>> GetAll(PaginParameter userParameter)
         {
             var appUsers = await _appUserRepository.GetAllUsers(userParameter);
             return Ok(appUsers);
         }*/

        public async Task<ActionResult<List<AppUserDTO>>> GetAll([FromQuery] PaginParameter userParameter)
        {
            List<AppUserDTO> appUsers = await _appUserRepository.GetAllUser(userParameter);
            return Ok(appUsers);
        }

        [HttpPatch("add-photo")]
        /*[Route("{id}")]*/
        public async Task<IActionResult> PhotoUpdate([FromForm] PhotoDTO photoDTO, string id)
        {
            var user = await _appUserRepository.AddPhoto(photoDTO, id);
            return Ok(user);
        }

    }
}
