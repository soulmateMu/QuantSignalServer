using Microsoft.EntityFrameworkCore;
using QuantSignalServer.Data;
using QuantSignalServer.Models;
using QuantSignalServer.DTOs;
using System.Threading.Tasks;

namespace QuantSignalServer.Services
{
    public class StrategyService
    {
        private readonly AppDbContext _context;

        public StrategyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, Strategy? Strategy, string Message)> CreateStrategyAsync(StrategyDTO strategyDto, int userId)
        {
            if (await _context.Strategies.AnyAsync(s => s.Name == strategyDto.Name && s.UserId == userId))
                return (false, null, "Strategy name already exists");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (false, null, "User not found");

            var strategy = new Strategy
            {
                Name = strategyDto.Name,
                UserId = userId,
                User = user,
                ForwardTargets = strategyDto.ForwardTargets?.Select(url => new ForwardTarget { Url = url }).ToList() ?? new List<ForwardTarget>()
            };
            _context.Strategies.Add(strategy);
            await _context.SaveChangesAsync();
            return (true, strategy, "Strategy created");
        }

        public async Task<List<Strategy>> GetStrategiesAsync(int userId)
        {
            return await _context.Strategies
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<(bool Success, Strategy? Strategy, string Message)> UpdateStrategyAsync(string name, StrategyDTO strategyDto, int userId)
        {
            var strategy = await _context.Strategies
                .FirstOrDefaultAsync(s => s.Name == name && s.UserId == userId);
            if (strategy == null)
                return (false, null, "Strategy not found");

            strategy.Name = strategyDto.Name; // 更新名称（需检查唯一性）
            strategy.ForwardTargets = strategyDto.ForwardTargets?.Select(url => new ForwardTarget { Url = url }).ToList() ?? new List<ForwardTarget>();
            if (strategy.Name != name && await _context.Strategies.AnyAsync(s => s.Name == strategyDto.Name && s.UserId == userId))
                return (false, null, "New strategy name already exists");

            await _context.SaveChangesAsync();
            return (true, strategy, "Strategy updated");
        }

        public async Task<(bool Success, string Message)> DeleteStrategyAsync(string name, int userId)
        {
            var strategy = await _context.Strategies
                .FirstOrDefaultAsync(s => s.Name == name && s.UserId == userId);
            if (strategy == null)
                return (false, "Strategy not found");

            _context.Strategies.Remove(strategy);
            await _context.SaveChangesAsync();
            return (true, "Strategy deleted");
        }
    }
}