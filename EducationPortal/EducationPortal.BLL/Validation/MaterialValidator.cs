namespace EducationPortal.BLL.Validation
{
    using System;
    using System.IO;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;

    public class MaterialValidator : AbstractValidator<MaterialDTO>
    {
        public MaterialValidator()
        {
            this.RuleSet("Common", () =>
            {
                this.RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .WithErrorCode("MaterialNameLength");

                this.RuleFor(x => x.Name.Length)
                    .LessThanOrEqualTo(DataSettings.MaterialNameMaxCharacterCount)
                    .WithErrorCode("MaterialNameLength");

                this.RuleFor(x => x.Name.Length)
                    .GreaterThanOrEqualTo(DataSettings.MaterialNameMinCharacterCount)
                    .WithErrorCode("MaterialNameLength");

                this.RuleFor(x => x.Url)
                    .Must(this.HasUrlFormat)
                    .WithErrorCode("MaterialUrlFormat");
            });

            this.RuleSet("Article", () =>
            {
                this.RuleFor(x => (x as ArticleDTO).PublicationDate)
                    .NotNull()
                    .NotEmpty()
                    .Must(this.IsValidDate)
                    .WithErrorCode("ArticlePublicationDateFormat");
            });

            this.RuleSet("Book", () =>
            {
                this.RuleFor(x => (x as BookDTO).AuthorNames)
                    .NotNull()
                    .NotEmpty()
                    .WithErrorCode("BookAuthorNamesEmpty");

                this.RuleFor(x => (x as BookDTO).AuthorNames.Length)
                    .LessThanOrEqualTo(DataSettings.BookAuthorNamesMaxCharacterCount)
                    .WithErrorCode("BookAuthorNamesMaxCharacterCount");

                this.RuleFor(x => x.Url)
                    .Must(this.HasExtension)
                    .WithErrorCode("BookUrlExtension");

                this.RuleFor(x => (x as BookDTO).Format)
                    .NotNull()
                    .NotEmpty()
                    .WithErrorCode("BookFormatLength");

                this.RuleFor(x => (x as BookDTO).Format.Length)
                    .LessThanOrEqualTo(DataSettings.BookFormatMaxCharacterCount)
                    .WithErrorCode("BookFormatLength");

                this.RuleFor(x => (x as BookDTO).Format.Length)
                    .GreaterThanOrEqualTo(DataSettings.BookFormatMinCharacterCount)
                    .WithErrorCode("BookFormatLength");

                this.RuleFor(x => (x as BookDTO).PageCount)
                    .NotNull()
                    .NotEmpty()
                    .Must(this.IsPositiveNumber)
                    .WithErrorCode("BookPageCountValue");

                this.RuleFor(x => (x as BookDTO).PublishingYear)
                    .NotNull()
                    .NotEmpty()
                    .Must(this.IsValidYear)
                    .WithErrorCode("BookPublishingYearValue");
            });

            this.RuleSet("Video", () =>
            {
                this.RuleFor(x => (x as VideoDTO).Duration)
                    .NotNull()
                    .NotEmpty()
                    .Must(this.IsPositiveNumber)
                    .WithErrorCode("VideoDurationValue");

                this.RuleFor(x => (x as VideoDTO).Quality)
                    .NotNull()
                    .NotEmpty()
                    .WithErrorCode("VideoQualityValue");

                this.RuleFor(x => (x as VideoDTO).Quality.Length)
                    .LessThanOrEqualTo(DataSettings.VideoQualityMaxCharacterCount)
                    .WithErrorCode("VideoQualityValue");

                this.RuleFor(x => (x as VideoDTO).Quality.Length)
                    .GreaterThanOrEqualTo(DataSettings.VideoQualityMinCharacterCount)
                    .WithErrorCode("VideoQualityValue");
            });
        }

        private bool HasUrlFormat(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri)
                              && (uri.Scheme == Uri.UriSchemeHttp
                              || uri.Scheme == Uri.UriSchemeHttps);
        }

        private bool IsValidDate(string dateStr)
        {
            var isDate = DateTime.TryParse(dateStr, out DateTime date);
            return isDate && date <= DateTime.Now;
        }

        private bool HasExtension(string url)
        {
            var slashCount = url.Length - url.Replace("/", string.Empty).Length;
            var backSlashCount = url.Length - url.Replace("\\", string.Empty).Length;

            return Path.HasExtension(url)
                && (slashCount > 2 || (backSlashCount == 2 && slashCount > 0));
        }

        private bool IsPositiveNumber(string numberStr)
        {
            var isNumber = int.TryParse(numberStr, out int number);
            return isNumber && number > 0;
        }

        private bool IsValidYear(string numberStr)
        {
            var isNumber = int.TryParse(numberStr, out int number);
            return isNumber
                && number > 0
                && number <= DateTime.Now.Year;
        }
    }
}
