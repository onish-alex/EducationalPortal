namespace EducationPortal.ConsoleUI.Validation
{
    using EducationPortal.BLL;
    using EducationPortal.BLL.DTO;

    public class CourseDataValidator
    {
        private CourseDTO course;

        public CourseDataValidator(CourseDTO course)
        {
            this.course = course;
        }

        public ValidationResult Validate()
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
            if (this.course.Name.Length < DataSettings.CourseNameMinCharacterCount
             || this.course.Name.Length > DataSettings.CourseNameMaxCharacterCount)
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
            if (this.course.Description.Length < DataSettings.CourseDescriptionMinCharacterCount
             || this.course.Description.Length > DataSettings.CourseDescriptionMaxCharacterCount)
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
