using Microsoft.AspNetCore.Mvc;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;

namespace SteamReports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewAppService _reviewAppService;

        public ReviewsController(IReviewAppService reviewAppService)
        {
            _reviewAppService = reviewAppService;
        }

        [HttpGet]
        public IActionResult GetReviews([FromQuery] SearchParametersViewModel searchParameters)
        {
            var response =  _reviewAppService.GetReviews(searchParameters);
           return response.Data.Count > 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet]
        [Route("/summary")]
        public IActionResult GetReviewsSummary()
        {
            return Ok(_reviewAppService.GetSummary());
        }
    }
}
