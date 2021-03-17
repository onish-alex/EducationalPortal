namespace EducationPortal.ConsoleUI.Utilities
{
    using EducationPortal.ConsoleUI.Resources;

    public static class ResourceHelper
    {
        public static string GetValidationString(string key)
        {
            return ValidationMessages.ResourceManager.GetString(key);
        }

        public static string GetMessageString(string key)
        {
            var result = OperationMessages.ResourceManager.GetString(key);

            if (result == null)
            {
                result = OutputHelper.GetValidationErrorString(key);
            }

            return result;
        }
    }
}
