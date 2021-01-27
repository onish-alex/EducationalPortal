using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL;
using EducationPortal.BLL.DTO;

namespace EducationPortal.ConsoleUI.Validation
{
    public class CourseDataValidator
    {
        private CourseDTO course;

        public CourseDataValidator(CourseDTO course)
        {
            this.course = course;
        }

        public ValidationResult Validate()
        {
            var result = ValidateName();

            if (!result.IsValid)
            {
                return result;
            }

            result = ValidateDescription();

            if (!result.IsValid)
            {
                return result;
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

        private ValidationResult ValidateName()
        {
            if (course.Name.Length < DataSettings.CourseNameMinCharacterCount
             || course.Name.Length > DataSettings.CourseNameMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format("Название курса должно быть длиной от {0} до {1} символов!",
                                             DataSettings.CourseNameMinCharacterCount,
                                             DataSettings.CourseNameMaxCharacterCount)
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

        private ValidationResult ValidateDescription()
        {
            if (course.Description.Length < DataSettings.CourseDescriptionMinCharacterCount
             || course.Description.Length > DataSettings.CourseDescriptionMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format("Описание курса должно быть длиной от {0} до {1} символов!",
                                             DataSettings.CourseDescriptionMinCharacterCount,
                                             DataSettings.CourseDescriptionMaxCharacterCount)
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }
    }
}
