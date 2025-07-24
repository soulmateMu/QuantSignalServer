using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantSignalServer.Data;
using QuantSignalServer.Services;
using Microsoft.EntityFrameworkCore; // 添加此行以支持 FirstOrDefaultAsync

namespace QuantSignalServer.Controllers
{
    [Authorize(Roles = "Admin")] // 仅限管理员角色
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AppDbContext _context;

        public AdminController(UserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPut("users/{username}/vip")]
        public async Task<IActionResult> SetVipStatus(string username, [FromQuery] bool isVip)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return NotFound("User not found");

            user.IsVIP = isVip;
            await _context.SaveChangesAsync();
            return Ok($"User {username} VIP status set to {isVip}");
        }
    }
}