using Microsoft.AspNetCore.Mvc;
using QuantSignalServer.Services;
using QuantSignalServer.DTOs;

namespace QuantSignalServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            var result = await _userService.RegisterAsync(userDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(new { Token = result.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto)
        {
            var result = await _userService.LoginAsync(userDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(new { Token = result.Token });
        }
    }
}