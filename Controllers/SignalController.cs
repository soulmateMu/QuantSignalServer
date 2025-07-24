using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantSignalServer.Services;
using QuantSignalServer.DTOs;

namespace QuantSignalServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SignalController : ControllerBase
    {
        private readonly SignalForwarder _signalForwarder;

        public SignalController(SignalForwarder signalForwarder)
        {
            _signalForwarder = signalForwarder;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal([FromBody] SignalDTO signalDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _signalForwarder.ForwardSignalAsync(signalDto, int.Parse(userId));
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok();
        }

        [HttpGet("{strategyName}")]
        public async Task<IActionResult> GetSignalHistory(string strategyName)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var signals = await _signalForwarder.GetSignalsAsync(strategyName, int.Parse(userId));
            return Ok(signals);
        }
    }
}