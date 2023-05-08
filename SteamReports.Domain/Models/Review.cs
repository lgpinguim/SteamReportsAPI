namespace SteamReports.Domain.Models
{
    public record Review
    {
        public long? Id { get; set; }
        public bool Recommended { get; set; }
        public string? UserName { get; set; }
        public string? ReviewText { get; set; }
        public float HoursPlayed { get; set; }
        public string? ReviewUrl { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int HelpfulAmount { get; set; }
        public int HelpfulTotal { get; set; }
        public int OwnedGamesAmount { get; set; }
        public long? RespondedBy { get; set; }
        public DateTime? RespondedTimeStamp { get; set; }
        public long? IssueList { get; set; }
        public bool CanBeTurned { get; set; }
        public long SteamAppId { get; set; }
        public string? LangKey { get; set; }
        public bool ReceivedCompensation { get; set; }

        public SteamApp? SteamApp { get; set; } 
    }
}
