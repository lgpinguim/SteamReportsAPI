using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Enums;
using System.Text.Json;
using Swashbuckle.AspNetCore.Annotations;
using static System.Net.Mime.MediaTypeNames;

namespace SteamReportsAPI.Controllers
{
    [Produces("application/json")]
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

        /// <summary>
        /// Gets a list containing the average player number and growth by timespan.
        /// </summary>
        /// <param name="timespan"></param>
        [HttpGet("trends")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<PlayerCountTrendViewModel>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public IActionResult GetTrends(TrendTimespanEnum timespan)
        {
            string cacheKey = $"trends{timespan}";

            List<PlayerCountTrendViewModel> responseList;

            var cacheTrendList = _cache.GetString(cacheKey);

            if (cacheTrendList != null)
                responseList = JsonSerializer.Deserialize<List<PlayerCountTrendViewModel>>(cacheTrendList)!;
            
            else
            {
                responseList = _playerCountAppService.GetPlayerCountTrendsByTimespan(timespan);
                var jsonResponseList = JsonSerializer.Serialize(responseList);
                _cache.SetString(cacheKey, jsonResponseList, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
            }

            return responseList.Count != 0 ? Ok(responseList) : NotFound();
        }
    }
}
