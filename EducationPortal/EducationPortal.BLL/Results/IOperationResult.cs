namespace EducationPortal.BLL.Results
{
    public interface IOperationResult
    {
        string MessageCode { get; set; }

        bool IsSuccessful { get; set; }
    }
}
