using SteamReports.Application.ViewModels;
using SteamReports.Domain.Core.Helpers.Pagination;
using SteamReports.Domain.Enums;
using SteamReports.Domain.Models;

namespace SteamReports.Application.Interfaces
{
    public interface IReviewAppService
    {
        PagedResponse<List<Review>> GetReviews(SearchParametersViewModel searchParameters);
        List<GameReviewViewModel> GetSummary();
    }
}
