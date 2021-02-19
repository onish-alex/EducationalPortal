namespace EducationPortal.ConsoleUI.Validation
{
    using EducationPortal.BLL;
    using EducationPortal.BLL.DTO;

    public class SkillDataValidator : Validator
    {
        public SkillDTO Skill { get; set; }

        public override string Name => "Skill";

        public override ValidationResult Validate()
        {
            var result = this.ValidateName();

            if (!result.IsValid)
            {
                return result;
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidateName()
        {
            if (this.Skill.Name.Length < DataSettings.SkillNameMinCharacterCount
             || this.Skill.Name.Length > DataSettings.SkillNameMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Название умения должно быть длиной от {0} до {1} символов!",
                        DataSettings.SkillNameMinCharacterCount,
                        DataSettings.SkillNameMaxCharacterCount),
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }
    }
}
