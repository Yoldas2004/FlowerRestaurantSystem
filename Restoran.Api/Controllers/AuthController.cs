using Microsoft.AspNetCore.Mvc;
using Restoran.Api.DTOs;

using Restoran.Business.Services;
using Restoran.Data.Enums;

namespace Restoran.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
             _userService = userService;
        }
        [HttpPost("register")]


        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try 
            {
                var register = await _userService.RegisterAsync(dto.UserName, dto.Password,RoleType.Waiter);
                return Ok(new { register.Id, register.UserName,register.Role });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }





        }
    }
}
