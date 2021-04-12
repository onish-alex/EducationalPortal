namespace EducationPortal.BLL.Utilities
{
    using System.Collections.Generic;

    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(IEnumerable<T> items, int page, int pageSize, int pageTotalCount)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.PageTotalCount = pageTotalCount;

            this.AddRange(items);
        }

        public int Page { get; }

        public int PageSize { get; }

        public int PageTotalCount { get; }

        public bool HasPreviousPage => this.Page > 1;

        public bool HasNextPage => this.Page < this.PageTotalCount;
    }
}
