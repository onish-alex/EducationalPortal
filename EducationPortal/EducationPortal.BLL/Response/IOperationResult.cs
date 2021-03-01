namespace EducationPortal.BLL.Response
{
    public interface IOperationResult
    {
        string MessageCode { get; set; }

        bool IsSuccessful { get; set; }
    }
}
