namespace SteamReports.Domain.Models
{
    public class SteamApp
    {
        public long SteamAppId { get; set; }
        public string? DisplayName { get; set; }

        public ICollection<Review> Reviews { get; set; }

    }
}
