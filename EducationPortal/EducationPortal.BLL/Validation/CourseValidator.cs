namespace EducationPortal.BLL.Validation
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;

    public class CourseValidator : AbstractValidator<CourseDTO>
    {
        public CourseValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("CourseNameLength");

            this.RuleFor(x => x.Name.Length)
                .GreaterThanOrEqualTo(DataSettings.CourseNameMinCharacterCount)
                .WithErrorCode("CourseNameLength");

            this.RuleFor(x => x.Name.Length)
               .LessThanOrEqualTo(DataSettings.CourseNameMaxCharacterCount)
               .WithErrorCode("CourseNameLength");

            this.RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("CourseDescriptionLength");

            this.RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(DataSettings.CourseDescriptionMaxCharacterCount)
                .WithErrorCode("CourseDescriptionLength");

            this.RuleFor(x => x.Description.Length)
                .GreaterThanOrEqualTo(DataSettings.CourseDescriptionMinCharacterCount)
                .WithErrorCode("CourseDescriptionLength");
        }
    }
}
