namespace SteamReports.Domain.Models
{
    public record Issue
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long ResolvedStatus { get; set; }

    }
}
