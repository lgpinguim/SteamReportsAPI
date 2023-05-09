namespace SteamReports.Application.ViewModels
{
    public class SearchParametersViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsDateFilterValid()
        {
            if ((!StartDate.HasValue && EndDate.HasValue) || (StartDate.HasValue && !EndDate.HasValue))
            {
                return false;
            }

            return !(EndDate < StartDate);
        }

        public bool HasDateFilter()
        {
            return EndDate.HasValue || StartDate.HasValue;
        }
    }
}
