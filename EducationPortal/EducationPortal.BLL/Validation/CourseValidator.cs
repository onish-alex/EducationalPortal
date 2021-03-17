namespace EducationPortal.BLL.Validation
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;
    using FluentValidation.Results;

    public class CourseValidator : AbstractValidator<CourseDTO>, IValidator<CourseDTO>
    {
        public CourseValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .WithErrorCode("CourseNameLength")
                .NotEmpty()
                .WithErrorCode("CourseNameLength");

            this.RuleFor(x => x.Name.Length)
                .GreaterThanOrEqualTo(DataSettings.CourseNameMinCharacterCount)
                .WithErrorCode("CourseNameLength")
                .LessThanOrEqualTo(DataSettings.CourseNameMaxCharacterCount)
                .WithErrorCode("CourseNameLength");

            this.RuleFor(x => x.Description)
                .NotNull()
                .WithErrorCode("CourseDescriptionLength")
                .NotEmpty()
                .WithErrorCode("CourseDescriptionLength");

            this.RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(DataSettings.CourseDescriptionMaxCharacterCount)
                .WithErrorCode("CourseDescriptionLength")
                .GreaterThanOrEqualTo(DataSettings.CourseDescriptionMinCharacterCount)
                .WithErrorCode("CourseDescriptionLength");
        }

        ValidationResult IValidator<CourseDTO>.Validate(CourseDTO model)
        {
            return this.Validate(model);
        }

        ValidationResult IValidator<CourseDTO>.Validate(CourseDTO model, params string[] ruleSetNames)
        {
            return this.Validate(model, opt => opt.IncludeRuleSets(ruleSetNames));
        }
    }
}
