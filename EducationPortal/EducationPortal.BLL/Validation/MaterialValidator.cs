namespace EducationPortal.BLL.Validation
{
    using System;
    using System.IO;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;
    using FluentValidation.Results;

    public class MaterialValidator : AbstractValidator<MaterialDTO>, IValidator<MaterialDTO>
    {
        public MaterialValidator()
        {
            this.RuleSet("Common", () =>
            {
                this.RuleFor(x => x.Name)
                    .NotNull()
                    .WithErrorCode("MaterialNameLength")
                    .NotEmpty()
                    .WithErrorCode("MaterialNameLength");

                this.RuleFor(x => x.Name.Length)
                    .LessThanOrEqualTo(DataSettings.MaterialNameMaxCharacterCount)
                    .WithErrorCode("MaterialNameLength")
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
                    .WithErrorCode("ArticlePublicationDateFormat")
                    .NotEmpty()
                    .WithErrorCode("ArticlePublicationDateFormat")
                    .Must(this.IsValidDate)
                    .WithErrorCode("ArticlePublicationDateFormat");
            });

            this.RuleSet("Book", () =>
            {
                this.RuleFor(x => (x as BookDTO).AuthorNames)
                    .NotNull()
                    .WithErrorCode("BookAuthorNamesEmpty")
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
                    .WithErrorCode("BookFormatLength")
                    .NotEmpty()
                    .WithErrorCode("BookFormatLength");

                this.RuleFor(x => (x as BookDTO).Format.Length)
                    .LessThanOrEqualTo(DataSettings.BookFormatMaxCharacterCount)
                    .WithErrorCode("BookFormatLength")
                    .GreaterThanOrEqualTo(DataSettings.BookFormatMinCharacterCount)
                    .WithErrorCode("BookFormatLength");

                this.RuleFor(x => (x as BookDTO).PageCount)
                    .NotNull()
                    .WithErrorCode("BookPageCountValue")
                    .NotEmpty()
                    .WithErrorCode("BookPageCountValue")
                    .Must(this.IsPositiveNumber)
                    .WithErrorCode("BookPageCountValue");

                this.RuleFor(x => (x as BookDTO).PublishingYear)
                    .NotNull()
                    .WithErrorCode("BookPublishingYearValue")
                    .NotEmpty()
                    .WithErrorCode("BookPublishingYearValue")
                    .Must(this.IsValidYear)
                    .WithErrorCode("BookPublishingYearValue");
            });

            this.RuleSet("Video", () =>
            {
                this.RuleFor(x => (x as VideoDTO).Duration)
                    .NotNull()
                    .WithErrorCode("VideoDurationValue")
                    .NotEmpty()
                    .WithErrorCode("VideoDurationValue")
                    .Must(this.IsPositiveNumber)
                    .WithErrorCode("VideoDurationValue");

                this.RuleFor(x => (x as VideoDTO).Quality)
                    .NotNull()
                    .WithErrorCode("VideoQualityValue")
                    .NotEmpty()
                    .WithErrorCode("VideoQualityValue");

                this.RuleFor(x => (x as VideoDTO).Quality.Length)
                    .LessThanOrEqualTo(DataSettings.VideoQualityMaxCharacterCount)
                    .WithErrorCode("VideoQualityValue")
                    .GreaterThanOrEqualTo(DataSettings.VideoQualityMinCharacterCount)
                    .WithErrorCode("VideoQualityValue");
            });
        }

        ValidationResult IValidator<MaterialDTO>.Validate(MaterialDTO model)
        {
            return this.Validate(model);
        }

        ValidationResult IValidator<MaterialDTO>.Validate(MaterialDTO model, params string[] ruleSetNames)
        {
            return this.Validate(model, opt => opt.IncludeRuleSets(ruleSetNames));
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
