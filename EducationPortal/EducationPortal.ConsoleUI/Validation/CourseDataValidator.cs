namespace EducationPortal.ConsoleUI.Validation
{
    using EducationPortal.BLL;
    using EducationPortal.BLL.DTO;

    public class CourseDataValidator : Validator
    {
        public CourseDTO Course { get; set; }

        public override string Name => "Course";

        public override ValidationResult Validate()
        {
            var result = this.ValidateName();

            if (!result.IsValid)
            {
                return result;
            }

            result = this.ValidateDescription();

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
            if (this.Course.Name.Length < DataSettings.CourseNameMinCharacterCount
             || this.Course.Name.Length > DataSettings.CourseNameMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Название курса должно быть длиной от {0} до {1} символов!",
                        DataSettings.CourseNameMinCharacterCount,
                        DataSettings.CourseNameMaxCharacterCount),
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidateDescription()
        {
            if (this.Course.Description.Length < DataSettings.CourseDescriptionMinCharacterCount
             || this.Course.Description.Length > DataSettings.CourseDescriptionMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Описание курса должно быть длиной от {0} до {1} символов!",
                        DataSettings.CourseDescriptionMinCharacterCount,
                        DataSettings.CourseDescriptionMaxCharacterCount),
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
