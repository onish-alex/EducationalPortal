namespace EducationPortal.MVC.Utilities
{
    using EducationPortal.BLL.Settings;
    using EducationPortal.MVC.Resources;
    using Microsoft.Extensions.Configuration;

    public class ResourceHelper : IResourceHelper
    {
        private IConfiguration configuration;

        public ResourceHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetMessageString(string key)
        {
            var result = OperationMessages.ResourceManager.GetString(key);

            if (result == null)
            {
                result = this.GetValidationErrorString(key);
            }

            return result;
        }

        public string GetValidationErrorString(string errorCode)
        {
            switch (errorCode)
            {
                case "AccountLoginLength":
                    return string.Format(
                        this.GetValidationString(errorCode),
                        this.configuration.GetValue<int>("AccountSettings:LoginMinLength"),
                        this.configuration.GetValue<int>("AccountSettings:LoginMaxLength"));

                case "AccountPasswordLength":
                    return string.Format(
                        this.GetValidationString(errorCode),
                        this.configuration.GetValue<int>("AccountSettings:PasswordMinLength"),
                        this.configuration.GetValue<int>("AccountSettings:PasswordMaxLength"));

                case "BookAuthorNamesLength":
                    return string.Format(
                        this.GetValidationString(errorCode),
                        DataSettings.BookAuthorNamesMaxCharacterCount);

                case "BookFormatLength":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            DataSettings.BookFormatMinCharacterCount,
                            DataSettings.BookFormatMaxCharacterCount);

                case "CourseDescriptionLength":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            DataSettings.CourseDescriptionMinCharacterCount,
                            DataSettings.CourseDescriptionMaxCharacterCount);

                case "CourseNameLength":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            DataSettings.CourseNameMinCharacterCount,
                            DataSettings.CourseNameMaxCharacterCount);

                case "MaterialNameLength":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            DataSettings.MaterialNameMinCharacterCount,
                            DataSettings.MaterialNameMaxCharacterCount);

                case "SkillNameLength":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            DataSettings.SkillNameMinCharacterCount,
                            DataSettings.SkillNameMaxCharacterCount);

                case "UserNameLength":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            this.configuration.GetValue<int>("AccountSettings:NameMinLength"),
                            this.configuration.GetValue<int>("AccountSettings:NameMaxLength"));

                case "VideoQualityValue":
                    return string.Format(
                            this.GetValidationString(errorCode),
                            DataSettings.VideoQualityMinCharacterCount,
                            DataSettings.VideoQualityMaxCharacterCount);

                default:
                    return this.GetValidationString(errorCode);
            }
        }

        public string GetCommonContentString(string key)
        {
            return CommonContent.ResourceManager.GetString(key);
        }

        private string GetValidationString(string key)
        {
            return ValidationMessages.ResourceManager.GetString(key);
        }
    }
}
