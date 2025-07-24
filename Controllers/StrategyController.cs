using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuantSignalServer.Data;
using QuantSignalServer.DTOs;
using QuantSignalServer.Models;
using System.Security.Claims;

namespace QuantSignalServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StrategyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StrategyController> _logger;

        public StrategyController(AppDbContext context, ILogger<StrategyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStrategy([FromBody] StrategyDTO strategyDto)
        {
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
            _logger.LogInformation($"JWT Claims: {string.Join(", ", claims)}");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogError("无法获取用户 ID，可能 JWT 验证失败");
                return Unauthorized("无法获取用户 ID");
            }
            if (!int.TryParse(userId, out int parsedUserId))
            {
                _logger.LogError($"无效的用户 ID 格式: {userId}");
                return BadRequest("无效的用户 ID 格式");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == parsedUserId);
            if (user == null)
            {
                _logger.LogError($"用户 ID {parsedUserId} 不存在");
                return Unauthorized($"用户 ID {parsedUserId} 不存在");
            }
            _logger.LogInformation($"用户 ID: {parsedUserId}, 用户名: {user.Username}, IsVIP: {user.IsVIP}");

            // 检查策略名称是否已存在
            if (await _context.Strategies.AnyAsync(s => s.Name == strategyDto.Name))
            {
                _logger.LogWarning($"策略名称 '{strategyDto.Name}' 已存在");
                return BadRequest($"策略名称 '{strategyDto.Name}' 已存在，请使用其他名称");
            }

            if (!user.IsVIP)
            {
                var count = await _context.Strategies.CountAsync(s => s.UserId == parsedUserId);
                if (count >= 3)
                    return BadRequest($"非 VIP 用户最多只能注册 3 个策略，当前已有 {count} 个，如需更多请升级为 VIP。");
            }

            var strategy = new Strategy
            {
                Name = strategyDto.Name,
                UserId = parsedUserId,
                User = user,
                ForwardTargets = strategyDto.ForwardTargets.Select(url => new ForwardTarget { Url = url }).ToList()
            };

            try
            {
                _context.Strategies.Add(strategy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
            {
                _logger.LogError(ex, $"保存策略失败，名称 '{strategyDto.Name}' 已存在");
                return BadRequest($"策略名称 '{strategyDto.Name}' 已存在，请使用其他名称");
            }

            var response = new StrategyResponseDTO
            {
                Id = strategy.Id,
                Name = strategy.Name,
                UserId = strategy.UserId,
                ForwardTargets = strategy.ForwardTargets.Select(ft => ft.Url).ToList()
            };
            return Ok(response);
        }
    }
}