namespace EducationPortal.ConsoleUI.Validation
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;

    public class RegisterDataValidator : Validator
    {
        public UserDTO User { get; set; }

        public AccountDTO Account { get; set; }

        public override string Name => "Register";

        public override ValidationResult Validate()
        {
            var result = this.ValidateLogin();

            if (!result.IsValid)
            {
                return result;
            }

            result = this.ValidatePassword();

            if (!result.IsValid)
            {
                return result;
            }

            result = this.ValidateEmail();

            if (!result.IsValid)
            {
                return result;
            }

            result = this.ValidateName();

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

        private ValidationResult ValidateLogin()
        {
            if (this.Account.Login.Length < DataSettings.UserLoginMinCharacterCount
             || this.Account.Login.Length > DataSettings.UserLoginMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Логин должен быть длиной от {0} до {1} символов!",
                        DataSettings.UserLoginMinCharacterCount,
                        DataSettings.UserLoginMaxCharacterCount),
                };
            }

            if (this.Account.Login.Any(symbol => !DataSettings.AllowableSymbols.Contains(char.ToLower(symbol))))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Логин может содержать только символы A-Z, a-z, А-Я, а-я и символ \"_\"",
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidatePassword()
        {
            if (this.Account.Password.Length < DataSettings.UserPasswordMinCharacterCount
             || this.Account.Password.Length > DataSettings.UserPasswordMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Пароль должен быть длиной от {0} до {1} символов!",
                        DataSettings.UserPasswordMinCharacterCount,
                        DataSettings.UserPasswordMaxCharacterCount),
                };
            }

            if (this.Account.Password.Any(symbol => !DataSettings.AllowableSymbols.Contains(char.ToLower(symbol))))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Пароль может содержать только символы A-Z, a-z, А-Я, а-я и символ \"_\"",
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidateEmail()
        {
            MailAddress email;
            try
            {
                email = new MailAddress(this.Account.Email);
            }
            catch (Exception)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Необходимо указать email!",
                };
            }

            if (email.Address.Contains("..")
             || email.Address[0] == '.'
             || email.Address[^1] == '.')
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Неверный формат email!",
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidateName()
        {
            if (this.User.Name.Length < DataSettings.UserLoginMinCharacterCount
             || this.User.Name.Length > DataSettings.UserLoginMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Имя должно быть длиной от {0} до {1} символов!",
                        DataSettings.UserNameMinCharacterCount,
                        DataSettings.UserNameMaxCharacterCount),
                };
            }

            if (this.User.Name.Any(symbol => !DataSettings.AllowableSymbols.Contains(char.ToLower(symbol))))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Имя может содержать только символы A-Z, a-z, А-Я, а-я и символ \"_\"",
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
