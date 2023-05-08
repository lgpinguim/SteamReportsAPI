using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Enums;
using SteamReports.Domain.Interfaces;

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

            int groupingInterval = (int)trendTimespan;

            var groupedData = steamDataList
                .GroupBy(data => new { data.SteamAppId, Grouping = data.TimeStamp.Date.AddDays(-(data.TimeStamp.Date.DayOfYear % groupingInterval)) })
                .OrderBy(groupData => groupData.Key.SteamAppId)
                .ThenBy(groupData => groupData.Key.Grouping);

            var trendList = new List<PlayerCountTrendViewModel>();

            for (int i = 0; i < groupedData.Count(); i++)
            {
                var currentGroup = groupedData.ElementAt(i);
                var trendViewModel = new PlayerCountTrendViewModel
                {
                    SteamAppId = currentGroup.Key.SteamAppId,
                    Date = currentGroup.Key.Grouping,
                    AveragePlayers = Math.Round(currentGroup.Average(d => d.PlayerCount))
                };

                if (HasPreviousElement(i))
                {
                    var previousElement = trendList.ElementAt(i - 1);

                    var isValid = ValidateGrowth(i, previousElement, trendViewModel);

                    if (!isValid)
                        trendViewModel.GrowthPercentage = 0;

                    else 
                        trendViewModel.GrowthPercentage = CalculateGrowthPercentage(trendViewModel, previousElement);

                }
                else
                {
                    trendViewModel.GrowthPercentage = 0;
                }

                trendList.Add(trendViewModel);
            }
            return trendList.ToList();
        }

        private static double CalculateGrowthPercentage(PlayerCountTrendViewModel trendViewModel, PlayerCountTrendViewModel previousElement)
        {
            return Math.Round((
                ((trendViewModel.AveragePlayers - previousElement.AveragePlayers) /
                 previousElement.AveragePlayers) * 100), 3);
        }

        private static bool ValidateGrowth(int i, PlayerCountTrendViewModel previousElement, PlayerCountTrendViewModel currentElement)
        {
            return previousElement.SteamAppId == currentElement.SteamAppId &&
                   previousElement.AveragePlayers != 0;
        }

        private static bool HasPreviousElement(int i)
        {
            return i != 0;
        }

    }
}
