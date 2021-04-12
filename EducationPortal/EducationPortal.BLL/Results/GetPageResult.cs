namespace EducationPortal.BLL.Results
{
    using EducationPortal.BLL.Utilities;

    public class GetPageResult<T> : OperationResult
    {
        public PaginatedList<T> PageList { get; set; }
    }
}
