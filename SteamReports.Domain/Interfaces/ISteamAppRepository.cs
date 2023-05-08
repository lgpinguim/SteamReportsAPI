using SteamReports.Domain.Models;

namespace SteamReports.Domain.Interfaces
{
    public interface ISteamAppRepository
    {
        List<SteamApp> GetAll();
    }
}
