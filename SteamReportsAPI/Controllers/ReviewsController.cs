using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using System.Text.Json;

namespace SteamReportsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewAppService _reviewAppService;
        private readonly IDistributedCache _cache;

        public ReviewsController(IReviewAppService reviewAppService, IDistributedCache cache)
        {
            _reviewAppService = reviewAppService;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult GetReviews([FromQuery] SearchParametersViewModel searchParameters)
        {
            var response =  _reviewAppService.GetReviews(searchParameters);
           return response.Data.Count > 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("summary")]
        public IActionResult GetReviewsSummary()
        {
            var cacheKey = "summary";

            List<GameReviewViewModel> responseList;

            var cacheSummaryList = _cache.GetString(cacheKey);

            if (cacheSummaryList != null)
                responseList = JsonSerializer.Deserialize<List<GameReviewViewModel>>(cacheSummaryList)!;

            else
            {
                responseList = _reviewAppService.GetSummary();
                var jsonResponseList = JsonSerializer.Serialize(responseList);
                _cache.SetString(cacheKey, jsonResponseList, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)});
            }

            return Ok(responseList);
        }
    }
}
