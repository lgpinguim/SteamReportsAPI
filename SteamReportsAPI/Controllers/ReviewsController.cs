using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Core.Helpers.Pagination;
using SteamReports.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace SteamReportsAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
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

        /// <summary>
        /// Gets reviews accordingly to the filters passed on the SearchParameters class.
        /// </summary>
        /// <param name="searchParameters"></param>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PagedResponse<List<Review>>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public IActionResult GetReviews([FromQuery] SearchParametersViewModel searchParameters)
        {
            if (searchParameters.HasDateFilter() && !searchParameters.IsDateFilterValid())
                return BadRequest("Invalid date filters");

            var response = _reviewAppService.GetReviews(searchParameters);
            return response.Data.Count > 0 ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Gets a summary of game reviews by SteamApp
        /// </summary>
        [HttpGet("summary")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<GameReviewViewModel>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
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
                _cache.SetString(cacheKey, jsonResponseList, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
            }

            return Ok(responseList);
        }
    }
}
