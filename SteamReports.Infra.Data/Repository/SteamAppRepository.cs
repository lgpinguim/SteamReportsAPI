using Microsoft.EntityFrameworkCore;
using SteamReports.Domain.Interfaces;
using SteamReports.Domain.Models;
using SteamReports.Infra.Data.Context;

namespace SteamReports.Infra.Data.Repository
{
    public class SteamAppRepository : ISteamAppRepository
    {
        protected readonly SteamReportsContext Db;
        protected readonly DbSet<SteamApp> DbSet;

        public SteamAppRepository(SteamReportsContext context)
        {
            Db = context;
            DbSet = Db.Set<SteamApp>();
        }

        public List<SteamApp> GetAll()
        {
          return DbSet.Include(r=>r.Reviews).ToList();
        }
    }
}
