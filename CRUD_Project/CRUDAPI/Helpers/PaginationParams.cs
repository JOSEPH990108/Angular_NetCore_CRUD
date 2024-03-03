
namespace CRUDAPI.Helpers
{
    public class PaginationParams
    {
        //Pagination Params
        private const int MAXPAGESIZE = 50;
        public int PageNumber { get; set; } = 1;
        public int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value;
        }
    }
}