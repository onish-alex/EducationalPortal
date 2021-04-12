namespace EducationPortal.MVC.Utilities
{
    public interface IResourceHelper
    {
        string GetMessageString(string key);

        string GetValidationErrorString(string errorCode);

        string GetCommonContentString(string key);
    }
}
