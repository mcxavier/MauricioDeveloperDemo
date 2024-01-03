using PagedList.Core;

namespace Core.SharedKernel
{
    public class PagerInfo
    {
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalRows { get; set; }

        public PagerInfo()
        {
            this.CurrentPage = null;
            this.PageSize = null;
            this.TotalRows = null;
            this.TotalPages = null;
        }

        public PagerInfo(int CurrentPage = 1, int PageSize = 1, int TotalRows = 0, int TotalPages = 1)
        {
            this.CurrentPage = CurrentPage;
            this.PageSize = PageSize;
            this.TotalRows = TotalRows;
            this.TotalPages = TotalPages;
        }

        public PagerInfo(IPagedList pagedList)
        {
            this.CurrentPage = pagedList.PageNumber;
            this.PageSize = pagedList.PageSize;
            this.TotalRows = pagedList.TotalItemCount;
            this.TotalPages = pagedList.PageCount;
        }
    }
}