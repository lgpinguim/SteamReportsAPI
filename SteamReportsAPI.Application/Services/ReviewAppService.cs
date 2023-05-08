using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Core.Helpers.Pagination;
using SteamReports.Domain.Interfaces;
using SteamReports.Domain.Models;

namespace SteamReports.Application.Services
{
    public class ReviewAppService : IReviewAppService
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly ISteamAppRepository _steamAppRepository;

        public ReviewAppService(IReviewRepository reviewRepository, ISteamAppRepository steamAppRepository)
        {
            _reviewRepository = reviewRepository;
            _steamAppRepository = steamAppRepository;
        }

        public PagedResponse<List<Review>> GetReviews(SearchParametersViewModel searchParameters)
        {
            var paginationFilter = new PaginationFilter(searchParameters.PageNumber, searchParameters.PageSize);

            var data = searchParameters.HasDateFilter()
                ? _reviewRepository.GetByDateRange(paginationFilter, searchParameters.StartDate!.Value,
                    searchParameters.EndDate!.Value, out var totalRecords)
                : _reviewRepository.GetAll(paginationFilter, out totalRecords);

            var (quotient, remainder) = Math.DivRem(totalRecords, paginationFilter.PageSize);

            var totalPages = quotient;
            if (remainder != 0)
                totalPages++;

            return new PagedResponse<List<Review>>(data, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalRecords);
        }

        public List<GameReviewViewModel> GetSummary()
        {
            var gamesList = _steamAppRepository.GetAll();

            return gamesList.Select(gr => new GameReviewViewModel()
            {
                SteamAppId = gr.SteamAppId,
                GameName = gamesList.FirstOrDefault(g => g.SteamAppId == gr.SteamAppId)!.DisplayName,
                PositiveReviews = gr.Reviews.Count(r => r.Recommended),
                NegativeReviews = gr.Reviews.Count(r => !r.Recommended),
                TotalReviews = gr.Reviews.Count,
            }).ToList();

        }
    }
}
