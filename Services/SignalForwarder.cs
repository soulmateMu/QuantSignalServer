using Microsoft.EntityFrameworkCore;
using QuantSignalServer.Data;
using QuantSignalServer.DTOs;
using QuantSignalServer.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace QuantSignalServer.Services
{
    public class SignalForwarder
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public SignalForwarder(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool Success, string Message)> ForwardSignalAsync(SignalDTO signalDto, int userId)
        {
            var strategy = await _context.Strategies
                .FirstOrDefaultAsync(s => s.Name == signalDto.StrategyName && s.UserId == userId);
            if (strategy == null)
                return (false, "Strategy not found or unauthorized");

            var signal = new Signal
            {
                StrategyName = signalDto.StrategyName,
                StockName = signalDto.StockName,
                StockCode = signalDto.StockCode,
                Direction = signalDto.Direction,
                Quantity = signalDto.Quantity,
                Value = signalDto.Value,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };
            _context.Signals.Add(signal);
            await _context.SaveChangesAsync();

            var httpClient = _httpClientFactory.CreateClient();
            foreach (var target in strategy.ForwardTargets)
            {
                try
                {
                    var json = JsonSerializer.Serialize(signalDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(target.Url, content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return (false, $"Failed to forward signal: {ex.Message}");
                }
            }
            return (true, "Signal forwarded successfully");
        }

        public async Task<List<Signal>> GetSignalsAsync(string strategyName, int userId)
        {
            var query = _context.Signals
                .Where(s => s.UserId == userId);
            if (!string.IsNullOrEmpty(strategyName))
                query = query.Where(s => s.StrategyName == strategyName);
            return await query.ToListAsync();
        }
    }
}