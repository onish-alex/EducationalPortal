namespace EducationPortal.BLL.Validation
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Net.Mail;
    using EducationPortal.BLL.DTO;
    using FluentValidation;

    public class AccountValidator : AbstractValidator<AccountDTO>
    {
        private NameValueCollection accountSettings;

        public AccountValidator()
        {
            this.accountSettings = ConfigurationManager.GetSection("accountSettings") as NameValueCollection;

            this.RuleFor(x => x.Login)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("AccountLoginLength");

            this.RuleFor(x => x.Login.Length)
                .LessThanOrEqualTo(int.Parse(this.accountSettings["LoginMaxLength"]))
                .WithErrorCode("AccountLoginLength");

            this.RuleFor(x => x.Login.Length)
                .GreaterThanOrEqualTo(int.Parse(this.accountSettings["LoginMinLength"]))
                .WithErrorCode("AccountLoginLength");

            this.RuleFor(x => x.Login)
                .Must(this.ContainOnlyAllowableSymbols)
                .WithErrorCode("AccountLoginUnallowableSymbols");

            this.RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .Must(this.IsValidEmail)
                .WithErrorCode("AccountEmailFormat");

            this.RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("AccountPasswordLength");

            this.RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(int.Parse(this.accountSettings["PasswordMinLength"]))
                .WithErrorCode("AccountPasswordLength");

            this.RuleFor(x => x.Password.Length)
                .LessThanOrEqualTo(int.Parse(this.accountSettings["PasswordMaxLength"]))
                .WithErrorCode("AccountPasswordLength");

            this.RuleFor(x => x.Password)
                .Must(this.ContainOnlyAllowableSymbols)
                .WithErrorCode("AccountPasswordUnallowableSymbols");
        }

        private bool ContainOnlyAllowableSymbols(string property)
        {
            var allowableSymbols = this.accountSettings["allowableSymbols"];
            return property.All(symbol => allowableSymbols.Contains(symbol));
        }

        private bool IsValidEmail(string emailStr)
        {
            MailAddress email;
            try
            {
                email = new MailAddress(emailStr);
            }
            catch (Exception)
            {
                return false;
            }

            if (email.Address.Contains("..")
             || email.Address[0] == '.'
             || email.Address[^1] == '.')
            {
                return false;
            }

            return true;
        }
    }
}
