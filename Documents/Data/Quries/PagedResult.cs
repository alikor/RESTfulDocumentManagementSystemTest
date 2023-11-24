namespace Documents.Data.Quries
{
    public class PagedResult<T>
    {
        public PagedResult(): this(0, 0, 0)
        {
            
        }
        public PagedResult(int totalCount, int pageNumber, int pageSize)
        {
            Documents = new List<T>();
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public IList<T> Documents { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage
        {
            get
            {
                return (PageNumber * PageSize) < TotalCount;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }
    }
}
