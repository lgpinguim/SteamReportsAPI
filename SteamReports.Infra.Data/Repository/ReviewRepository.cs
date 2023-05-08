using Microsoft.EntityFrameworkCore;
using SteamReports.Domain.Core.Helpers.Pagination;
using SteamReports.Domain.Interfaces;
using SteamReports.Domain.Models;
using SteamReports.Infra.Data.Context;

namespace SteamReports.Infra.Data.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        protected readonly SteamReportsContext Db;
        protected readonly DbSet<Review> DbSet;

        public ReviewRepository(SteamReportsContext context)
        {
            Db = context;
            DbSet = Db.Set<Review>();
        }

        public List<Review> GetAll(PaginationFilter paginationFilter, out int totalRecords)
        {
            totalRecords = DbSet.Count();
            return AddPagination(paginationFilter,DbSet).ToList();
        }

        public List<Review> GetByDateRange(PaginationFilter paginationFilter, DateTime startDate, DateTime endDate, out int totalRecords)
        {
            var records = DbSet.Where(r => r.DatePosted >= startDate && r.DatePosted <= endDate);
            totalRecords = records.Count();
            return AddPagination(paginationFilter, records).ToList();

        }

        private static IQueryable<Review> AddPagination(PaginationFilter paginationFilter, IQueryable<Review> records)
        {
            return records.OrderByDescending(r => r.DatePosted)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);
        }
    }
}
