namespace SteamReports.Application.ViewModels
{
    public class PlayerCountTrendViewModel
    {
        public long SteamAppId { get; set; }
        public DateTime Date { get; set; }
        public double AveragePlayers { get; set; }
        public double GrowthPercentage { get; set; }
    }
}
