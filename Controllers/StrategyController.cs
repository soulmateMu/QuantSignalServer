using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantSignalServer.Services;
using QuantSignalServer.DTOs;
using QuantSignalServer.Data;
using Microsoft.EntityFrameworkCore;

namespace QuantSignalServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StrategyController : ControllerBase
    {
        private readonly StrategyService _strategyService;
        private readonly AppDbContext _context; // 新增

        public StrategyController(StrategyService strategyService, AppDbContext context) // 修改构造函数
        {
            _strategyService = strategyService;
            _context = context; // 新增
        }

        [HttpPost]
        public async Task<IActionResult> CreateStrategy([FromBody] StrategyDTO strategyDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            // 查询用户
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
                return Unauthorized();

            // 非VIP用户最多只能注册3个策略
            if (!user.IsVIP)
            {
                var count = await _context.Strategies.CountAsync(s => s.UserId == user.Id);
                if (count >= 3)
                    return BadRequest("非VIP用户最多只能注册3个策略，如需更多请升级为VIP。");
            }

            var result = await _strategyService.CreateStrategyAsync(strategyDto, int.Parse(userId));
            if (!result.Success || result.Strategy == null)
                return BadRequest(result.Message);
            return Ok(result.Strategy);
        }

        [HttpGet]
        public async Task<IActionResult> GetStrategies()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var strategies = await _strategyService.GetStrategiesAsync(int.Parse(userId));
            return Ok(strategies);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStrategy(string id, [FromBody] StrategyDTO strategyDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _strategyService.UpdateStrategyAsync(id, strategyDto, int.Parse(userId));
            if (!result.Success || result.Strategy == null)
                return BadRequest(result.Message);
            return Ok(result.Strategy);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStrategy(string id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _strategyService.DeleteStrategyAsync(id, int.Parse(userId));
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }
    }
}