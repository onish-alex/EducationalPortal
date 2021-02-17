namespace EducationPortal.BLL.Response
{
    public interface IResponse
    {
        string Message { get; set; }

        bool IsSuccessful { get; set; }
    }
}
