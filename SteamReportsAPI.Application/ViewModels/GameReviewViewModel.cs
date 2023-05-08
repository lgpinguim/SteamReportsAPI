namespace SteamReports.Application.ViewModels
{
    public class GameReviewViewModel
    {
        public long? SteamAppId { get; set; }
        public string? GameName { get; set; }
        public int? PositiveReviews { get; set; }
        public int? NegativeReviews { get; set; }
        public int? TotalReviews { get; set; }
    }
}
