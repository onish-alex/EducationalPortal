using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL;

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
            var result = ValidateLogin();
            
            if (!result.IsValid)
            {
                return result;
            }

            result = ValidatePassword();
            
            if (!result.IsValid)
            {
                return result;
            }

            result = ValidateEmail();

            if (!result.IsValid)
            {
                return result;
            }

            result = ValidateName();

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

        private ValidationResult ValidateLogin()
        {
            if (user.account.Login.Length < DataSettings.UserLoginMinCharacterCount
             || user.account.Login.Length > DataSettings.UserLoginMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format("Логин должен быть длиной от {0} до {1} символов!",
                                             DataSettings.UserLoginMinCharacterCount,
                                             DataSettings.UserLoginMaxCharacterCount)
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

        private ValidationResult ValidatePassword()
        {
            if (user.account.Password.Length < DataSettings.UserPasswordMinCharacterCount
             || user.account.Password.Length > DataSettings.UserPasswordMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format("Пароль должен быть длиной от {0} до {1} символов!",
                                             DataSettings.UserPasswordMinCharacterCount,
                                             DataSettings.UserPasswordMaxCharacterCount)
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

        private ValidationResult ValidateEmail()
        {
            MailAddress email;
            try
            {
                email = new MailAddress(user.account.Email);
            }
            catch (ArgumentException)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Необходимо указать email!"
                };
            }
            catch
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Неверный формат email!"
                };
            }

            if (email.Address.Contains("..")
             || email.Address[0] == '.'
             || email.Address[email.Address.Length - 1] == '.')
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Неверный формат email!"
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

        private ValidationResult ValidateName()
        {
            if (user.userInfo.Name.Length < DataSettings.UserLoginMinCharacterCount
             || user.account.Login.Length > DataSettings.UserLoginMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format("Имя должно быть длиной от {0} до {1} символов!",
                                             DataSettings.UserNameMinCharacterCount,
                                             DataSettings.UserNameMaxCharacterCount)
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
