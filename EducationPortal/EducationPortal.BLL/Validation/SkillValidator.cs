namespace EducationPortal.BLL.Validation
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;
    using FluentValidation.Results;

    public class SkillValidator : AbstractValidator<SkillDTO>, IValidator<SkillDTO>
    {
        public SkillValidator()
        {
            this.RuleSet("Base", () =>
            {
                this.RuleFor(x => x.Name)
                .NotNull()
                .WithErrorCode("SkillNameNull")
                .NotEmpty()
                .WithErrorCode("SkillNameNull");
            });

            this.RuleSet("Detail", () =>
            {
                this.RuleFor(x => x.Name.Length)
                    .LessThanOrEqualTo(DataSettings.SkillNameMaxCharacterCount)
                    .WithErrorCode("SkillNameLength")
                    .GreaterThanOrEqualTo(DataSettings.SkillNameMinCharacterCount)
                    .WithErrorCode("SkillNameLength");
            });
        }

        ValidationResult IValidator<SkillDTO>.Validate(SkillDTO model)
        {
            return this.Validate(model);
        }

        ValidationResult IValidator<SkillDTO>.Validate(SkillDTO model, params string[] ruleSetNames)
        {
            return this.Validate(model, opt => opt.IncludeRuleSets(ruleSetNames));
        }
    }
}
