using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Enums;
using System.Text.Json;

namespace SteamReportsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerCountAppService _playerCountAppService;
        private readonly IDistributedCache _cache;


        public PlayersController(IPlayerCountAppService playerCountAppService, IDistributedCache cache)
        {
            _playerCountAppService = playerCountAppService;
            _cache = cache;
        }

        [HttpGet("trends")]
        public IActionResult GetTrends(TrendTimespanEnum timespan)
        {
            string cacheKey = $"trends{timespan}";

            List<PlayerCountTrendViewModel> responseList;

            var cacheTrendList = _cache.GetString(cacheKey);

            if (cacheTrendList != null)
            {
                responseList = JsonSerializer.Deserialize<List<PlayerCountTrendViewModel>>(cacheTrendList)!;
            }

            else
            {
                responseList = _playerCountAppService.GetPlayerCountTrendsByTimespan(timespan);
                var jsonResponseList = JsonSerializer.Serialize(responseList);
                _cache.SetString(cacheKey, jsonResponseList);
            }

            return Ok(responseList);
        }
    }
}
