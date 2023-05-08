using SteamReports.Domain.Models;

namespace SteamReports.Domain.Interfaces
{
    public interface ISteamPlayerCountRepository
    {
        List<SteamPlayerCount> GetAll();
    }
}
