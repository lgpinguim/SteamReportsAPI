using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SteamReports.Application.Interfaces;
using SteamReports.Domain.Enums;

namespace SteamReportsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerCountAppService _playerCountAppService;
        private readonly IMemoryCache _cache; 


        public PlayersController(IPlayerCountAppService playerCountAppService, IMemoryCache cache)
        {
            _playerCountAppService = playerCountAppService;
            _cache = cache;
        }

        [HttpGet("trends")]
        public IActionResult GetTrends(TrendTimespanEnum timespan)
        {
            // Retrieve the cached result or perform the expensive operation if not cached
            if (_cache.TryGetValue("trends", out var response)) return Ok(response);
            response = _playerCountAppService.GetPlayerCountTrendsByTimespan(timespan);
            _cache.Set("trends", response, TimeSpan.FromMinutes(15));

            return Ok(response);
        }
    }
}
