using Microsoft.AspNetCore.Mvc;
using SteamReports.Application.Interfaces;
using SteamReports.Domain.Enums;

namespace SteamReportsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerCountAppService _playerCountAppService;


        public PlayersController(IPlayerCountAppService playerCountAppService)
        {
            _playerCountAppService = playerCountAppService;
        }

        [HttpGet("trends")]
        public IActionResult GetTrends(TrendTimespanEnum timespan)
        {
            var response = _playerCountAppService.GetPlayerCountTrendsByTimespan(timespan);

            return Ok(response);
        }
    }
}
