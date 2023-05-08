using SteamReports.Domain.Core.Helpers.Pagination;
using SteamReports.Domain.Models;

namespace SteamReports.Domain.Interfaces
{
    public interface IReviewRepository
    {
        List<Review> GetAll(PaginationFilter paginationFilter, out int totalRecords);
        List<Review> GetByDateRange(PaginationFilter paginationFilter, DateTime startDate, DateTime endDate, out int totalRecords);
    }
}
