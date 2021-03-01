namespace EducationPortal.BLL.Validation
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;

    public class SkillValidator : AbstractValidator<SkillDTO>
    {
        public SkillValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("SkillNameLength");

            this.RuleFor(x => x.Name.Length)
                .LessThanOrEqualTo(DataSettings.SkillNameMaxCharacterCount)
                .WithErrorCode("SkillNameLength");

            this.RuleFor(x => x.Name.Length)
                .GreaterThanOrEqualTo(DataSettings.SkillNameMinCharacterCount)
                .WithErrorCode("SkillNameLength");
        }
    }
}
