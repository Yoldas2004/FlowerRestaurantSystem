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
                var response = new UserResponseDto 
                {
                    Id =register.Id,
                    Role = register.Role,
                    UserName = register.UserName,
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try 
            {
                var logister = await _userService.LoginAsync(dto.UserName,dto.Password);
               var response = new UserResponseDto { Id = logister.Id,  UserName = logister.UserName , Role = logister.Role};
                return Ok(response);



            }
            catch (ArgumentException ex)
            { 
                return BadRequest($"Failed to login {ex.Message}");
            }
        
        
        }
    }
}
