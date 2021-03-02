namespace EducationPortal.ConsoleUI.Utilities
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Text;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using EducationPortal.ConsoleUI.Resources;

    public static class OutputHelper
    {
        public static void PrintCourses(CourseDTO[] courses)
        {
            for (int i = 0; i < courses.Length; i++)
            {
                Console.WriteLine(
                    ConsoleMessages.OutputCourseTemplate,
                    i + 1,
                    courses[i].Name,
                    courses[i].Description,
                    string.Join(", ", courses[i].SkillNames));
            }
        }

        public static void PrintMaterials(MaterialDTO[] materials)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                Console.WriteLine(
                    ConsoleMessages.OutputMaterialTemplate,
                    i + 1,
                    GetMaterialString(materials[i]));
            }
        }

        public static void PrintValidationError(string errorCode)
        {
            Console.WriteLine(GetValidationErrorString(errorCode));
        }

        public static void PrintSingleMaterial(MaterialDTO material)
        {
            Console.WriteLine(GetMaterialString(material));
        }

        public static string GetValidationErrorString(string errorCode)
        {
            switch (errorCode)
            {
                case "AccountLoginLength":
                    return string.Format(
                        ResourceHelper.GetValidationString(errorCode),
                        (ConfigurationManager.GetSection("accountSettings") as NameValueCollection)["LoginMinLength"],
                        (ConfigurationManager.GetSection("accountSettings") as NameValueCollection)["LoginMaxLength"]);

                case "AccountPasswordLength":
                    return string.Format(
                        ResourceHelper.GetValidationString(errorCode),
                        (ConfigurationManager.GetSection("accountSettings") as NameValueCollection)["PasswordMinLength"],
                        (ConfigurationManager.GetSection("accountSettings") as NameValueCollection)["PasswordMaxLength"]);

                case "BookAuthorNamesLength":
                    return string.Format(
                        ResourceHelper.GetValidationString(errorCode),
                        DataSettings.BookAuthorNamesMaxCharacterCount);

                case "BookFormatLength":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            DataSettings.BookFormatMinCharacterCount,
                            DataSettings.BookFormatMaxCharacterCount);

                case "CourseDescriptionLength":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            DataSettings.CourseDescriptionMinCharacterCount,
                            DataSettings.CourseDescriptionMaxCharacterCount);

                case "CourseNameLength":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            DataSettings.CourseNameMinCharacterCount,
                            DataSettings.CourseNameMaxCharacterCount);

                case "MaterialNameLength":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            DataSettings.MaterialNameMinCharacterCount,
                            DataSettings.MaterialNameMaxCharacterCount);

                case "SkillNameLength":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            DataSettings.SkillNameMinCharacterCount,
                            DataSettings.SkillNameMaxCharacterCount);

                case "UserNameLength":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            (ConfigurationManager.GetSection("accountSettings") as NameValueCollection)["NameMinLength"],
                            (ConfigurationManager.GetSection("accountSettings") as NameValueCollection)["NameMaxLength"]);

                case "VideoQualityValue":
                    return string.Format(
                            ResourceHelper.GetValidationString(errorCode),
                            DataSettings.VideoQualityMinCharacterCount,
                            DataSettings.VideoQualityMaxCharacterCount);

                default:
                    return ResourceHelper.GetValidationString(errorCode);
            }
        }

        private static string GetMaterialString(MaterialDTO material)
        {
            var builder = new StringBuilder();
            builder.AppendFormat(ConsoleMessages.InfoMaterialName, material.Name);
            builder.AppendLine();

            builder.AppendFormat(ConsoleMessages.InfoMaterialUrl, material.Url);
            builder.AppendLine();

            switch (material)
            {
                case ArticleDTO article:
                    builder.AppendFormat(ConsoleMessages.InfoArticlePublicationDate, article.PublicationDate);
                    builder.AppendLine();
                    break;

                case BookDTO book:
                    builder.AppendFormat(ConsoleMessages.InfoBookAuthorNames, book.AuthorNames);
                    builder.AppendLine();

                    builder.AppendFormat(ConsoleMessages.InfoBookFormat, book.Format);
                    builder.AppendLine();

                    builder.AppendFormat(ConsoleMessages.InfoBookPublishingYear, book.PublishingYear);
                    builder.AppendLine();
                    break;

                case VideoDTO video:
                    builder.AppendFormat(ConsoleMessages.InfoVideoDuration, video.Duration);
                    builder.AppendLine();

                    builder.AppendFormat(ConsoleMessages.InfoVideoQuality, video.Quality);
                    builder.AppendLine();
                    break;
            }

            return builder.ToString();
        }
    }
}
