using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;

namespace EducationPortal.ConsoleUI.Validation
{
    public class RegisterDataValidator
    {
        private UserDTO user;

        public RegisterDataValidator(UserDTO user)
        {
            this.user = user;
        }

        public ValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
