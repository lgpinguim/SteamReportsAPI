using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Enums;

namespace SteamReports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewAppService _reviewAppService;
        private readonly IMemoryCache _cache;


        public ReviewsController(IReviewAppService reviewAppService, IMemoryCache cache)
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
            if (_cache.TryGetValue("summary", out var response)) return Ok(response);
            response = _reviewAppService.GetSummary();
            _cache.Set("summary", response, TimeSpan.FromMinutes(5));
            return Ok(response);
        }


    }
}
