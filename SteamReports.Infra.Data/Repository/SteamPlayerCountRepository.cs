using Microsoft.EntityFrameworkCore;
using SteamReports.Domain.Interfaces;
using SteamReports.Domain.Models;
using SteamReports.Infra.Data.Context;

namespace SteamReports.Infra.Data.Repository
{
    public class SteamPlayerCountRepository : ISteamPlayerCountRepository
    {
        protected readonly SteamReportsContext Db;
        protected readonly DbSet<SteamPlayerCount> DbSet;

        public SteamPlayerCountRepository(SteamReportsContext db)
        {
            Db = db;
            DbSet = Db.Set<SteamPlayerCount>();
        }

        public List<SteamPlayerCount> GetAll()
        {
          return  DbSet.ToList();
        }
    }
}
