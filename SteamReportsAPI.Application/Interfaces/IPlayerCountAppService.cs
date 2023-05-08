using SteamReports.Application.ViewModels;
using SteamReports.Domain.Enums;

namespace SteamReports.Application.Interfaces
{
    public interface IPlayerCountAppService
    {
        List<PlayerCountTrendViewModel> GetPlayerCountTrendsByTimespan(TrendTimespanEnum trendTimespan);
    }
}
