using System;
using System.Net.Mail;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL;

namespace EducationPortal.ConsoleUI.Validation
{
    public class RegisterDataValidator
    {
        private UserDTO user;
        private AccountDTO account;

        public RegisterDataValidator(UserDTO user, AccountDTO account)
        {
            this.user = user;
            this.account = account;
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
            if (account.Login.Length < DataSettings.UserLoginMinCharacterCount
             || account.Login.Length > DataSettings.UserLoginMaxCharacterCount)
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
            if (account.Password.Length < DataSettings.UserPasswordMinCharacterCount
             || account.Password.Length > DataSettings.UserPasswordMaxCharacterCount)
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
                email = new MailAddress(account.Email);
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
            if (user.Name.Length < DataSettings.UserLoginMinCharacterCount
             || user.Name.Length > DataSettings.UserLoginMaxCharacterCount)
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
