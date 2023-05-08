namespace SteamReports.Domain.Models
{
    public class SteamPlayerCount
    {
        public DateTime TimeStamp { get; set; }
        public long SteamAppId { get; set; }
        public int PlayerCount { get; set; }
    }
}
