using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Enums;
using SteamReports.Domain.Interfaces;
using System.Linq;

namespace SteamReports.Application.Services
{
    public class PlayerCountAppService : IPlayerCountAppService
    {
        private readonly ISteamPlayerCountRepository _playerCountRepository;

        public PlayerCountAppService(ISteamPlayerCountRepository playerCountRepository)
        {
            _playerCountRepository = playerCountRepository;
        }

        public List<PlayerCountTrendViewModel> GetPlayerCountTrendsByTimespan(TrendTimespanEnum trendTimespan)
        {
            var steamDataList = _playerCountRepository.GetAll();

            int groupingInterval = trendTimespan switch
            {
                TrendTimespanEnum.Daily => 1 // Group by day
                ,
                TrendTimespanEnum.Weekly => 7 // Group by week (7 days)
                ,
                TrendTimespanEnum.Yearly => 365 // Group by year (365 days)
                ,
                _ => throw new ArgumentOutOfRangeException(nameof(trendTimespan), trendTimespan, null)
            };

            var groupedData = steamDataList
                .GroupBy(data => new { data.SteamAppId, Grouping = data.TimeStamp.Date.AddDays(-(data.TimeStamp.Date.DayOfYear % groupingInterval)) })
                .OrderBy(groupData => groupData.Key.SteamAppId)
                .ThenBy(groupData => groupData.Key.Grouping);

            var trendList = new List<PlayerCountTrendViewModel>();

            for (int i = 0; i < groupedData.Count(); i++)
            {

                var group = groupedData.ElementAt(i);

                var trendViewModel = new PlayerCountTrendViewModel
                {
                    SteamAppId = group.Key.SteamAppId,
                    Date = group.Key.Grouping,
                    AveragePlayers = Math.Round(group.Average(d => d.PlayerCount))
                };

                if (i == 0)
                {
                    trendViewModel.GrowthPercentage = 0;
                    trendList.Add(trendViewModel);
                    continue;
                }

                var previousElement = trendList.ElementAt(i - 1);

                if (previousElement.SteamAppId != trendViewModel.SteamAppId || previousElement.AveragePlayers == 0)
                {
                    trendViewModel.GrowthPercentage = 0;
                    trendList.Add(trendViewModel);
                    continue;
                }

                trendViewModel.GrowthPercentage = Math.Round((
                    ((trendViewModel.AveragePlayers - previousElement.AveragePlayers) /
                     previousElement.AveragePlayers) * 100), 3);

                trendList.Add(trendViewModel);
            }


            return trendList.ToList();

        }

    }
}
