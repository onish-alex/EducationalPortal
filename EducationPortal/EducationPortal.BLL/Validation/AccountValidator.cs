namespace EducationPortal.BLL.Validation
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Net.Mail;
    using EducationPortal.BLL.DTO;
    using FluentValidation;
    using FluentValidation.Results;

    public class AccountValidator : AbstractValidator<AccountDTO>, IValidator<AccountDTO>
    {
        private NameValueCollection accountSettings;

        public AccountValidator()
        {
            this.accountSettings = ConfigurationManager.GetSection("accountSettings") as NameValueCollection;

            this.RuleSet("Base", () =>
            {
                this.RuleFor(x => x.Login)
                .NotNull()
                .WithErrorCode("AccountLoginNull")
                .NotEmpty()
                .WithErrorCode("AccountLoginNull");

                this.RuleFor(x => x.Email)
                .NotNull()
                .WithErrorCode("AccountEmailNull")
                .NotEmpty()
                .WithErrorCode("AccountEmailNull");

                this.RuleFor(x => x.Password)
                .NotNull()
                .WithErrorCode("AccountPasswordNull")
                .NotEmpty()
                .WithErrorCode("AccountPasswordNull");
            });

            this.RuleSet("Detail", () =>
            {
                this.RuleFor(x => x.Login.Length)
                    .LessThanOrEqualTo(int.Parse(this.accountSettings["LoginMaxLength"]))
                    .WithErrorCode("AccountLoginLength")
                    .GreaterThanOrEqualTo(int.Parse(this.accountSettings["LoginMinLength"]))
                    .WithErrorCode("AccountLoginLength");

                this.RuleFor(x => x.Login)
                    .Must(this.ContainOnlyAllowableSymbols)
                    .WithErrorCode("AccountLoginUnallowableSymbols");

                this.RuleFor(x => x.Email)
                    .Must(this.IsValidEmail)
                    .WithErrorCode("AccountEmailFormat");

                this.RuleFor(x => x.Password.Length)
                    .GreaterThanOrEqualTo(int.Parse(this.accountSettings["PasswordMinLength"]))
                    .WithErrorCode("AccountPasswordLength")
                    .LessThanOrEqualTo(int.Parse(this.accountSettings["PasswordMaxLength"]))
                    .WithErrorCode("AccountPasswordLength");

                this.RuleFor(x => x.Password)
                    .Must(this.ContainOnlyAllowableSymbols)
                    .WithErrorCode("AccountPasswordUnallowableSymbols");
            });
        }

        ValidationResult IValidator<AccountDTO>.Validate(AccountDTO model)
        {
            return this.Validate(model);
        }

        ValidationResult IValidator<AccountDTO>.Validate(AccountDTO model, params string[] ruleSetNames)
        {
            return this.Validate(model, opt => opt.IncludeRuleSets(ruleSetNames));
        }

        private bool ContainOnlyAllowableSymbols(string property)
        {
            var allowableSymbols = this.accountSettings["allowableSymbols"];
            return property.All(symbol => allowableSymbols.Contains(char.ToLower(symbol)));
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
