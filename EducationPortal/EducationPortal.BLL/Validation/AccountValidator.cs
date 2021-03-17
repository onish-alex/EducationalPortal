namespace EducationPortal.BLL.Validation
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Net.Mail;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.Extensions.Options;

    public class AccountValidator : AbstractValidator<AccountDTO>, IValidator<AccountDTO>
    {
        private NameValueCollection accountSettingsCollection;
        private AccountSettings accountSettings;

        public AccountValidator()
        {
            this.accountSettingsCollection = ConfigurationManager.GetSection("accountSettings") as NameValueCollection;

            this.SetBaseRules();

            this.RuleSet("Detail", () =>
            {
                this.RuleFor(x => x.Login)
                .NotEmpty()
                .WithErrorCode("AccountLoginNull")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Login.Length)
                    .LessThanOrEqualTo(int.Parse(this.accountSettingsCollection["LoginMaxLength"]))
                    .WithErrorCode("AccountLoginLength")
                    .GreaterThanOrEqualTo(int.Parse(this.accountSettingsCollection["LoginMinLength"]))
                    .WithErrorCode("AccountLoginLength");

                    this.RuleFor(x => x.Login)
                    .Must(this.ContainOnlyAllowableSymbols)
                    .WithErrorCode("AccountLoginUnallowableSymbols");
                });

                this.RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode("AccountEmailNull")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Email)
                        .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                        .WithErrorCode("AccountEmailFormat");
                });

                this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode("AccountPasswordNull")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Password.Length)
                        .GreaterThanOrEqualTo(int.Parse(this.accountSettingsCollection["PasswordMinLength"]))
                        .WithErrorCode("AccountPasswordLength")
                        .LessThanOrEqualTo(int.Parse(this.accountSettingsCollection["PasswordMaxLength"]))
                        .WithErrorCode("AccountPasswordLength");

                    this.RuleFor(x => x.Password)
                        .Must(this.ContainOnlyAllowableSymbols)
                        .WithErrorCode("AccountPasswordUnallowableSymbols");
                });
            });
        }

        public AccountValidator(IOptionsSnapshot<AccountSettings> options)
        {
            this.accountSettings = options.Value;

            this.SetBaseRules();

            this.RuleSet("Detail", () =>
            {
                this.RuleFor(x => x.Login)
                .NotEmpty()
                .WithErrorCode("AccountLoginNull")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Login.Length)
                        .LessThanOrEqualTo(this.accountSettings.LoginMaxLength)
                        .WithErrorCode("AccountLoginLength")
                        .GreaterThanOrEqualTo(this.accountSettings.LoginMinLength)
                        .WithErrorCode("AccountLoginLength");

                    this.RuleFor(x => x.Login)
                        .Must(this.ContainOnlyAllowableSymbols)
                        .WithErrorCode("AccountLoginUnallowableSymbols");
                });

                this.RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode("AccountEmailNull")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Email)
                    .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                    .WithErrorCode("AccountEmailFormat");
                });

                this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode("AccountPasswordNull")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Password.Length)
                        .GreaterThanOrEqualTo(this.accountSettings.PasswordMinLength)
                        .WithErrorCode("AccountPasswordLength")
                        .LessThanOrEqualTo(this.accountSettings.PasswordMaxLength)
                        .WithErrorCode("AccountPasswordLength");

                    this.RuleFor(x => x.Password)
                        .Must(this.ContainOnlyAllowableSymbols)
                        .WithErrorCode("AccountPasswordUnallowableSymbols");
                });
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

        private void SetBaseRules()
        {
            this.RuleSet("Base", () =>
            {
                this.RuleFor(x => x.Login)
                .NotEmpty()
                .WithErrorCode("AccountLoginNull");

                this.RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode("AccountEmailNull");

                this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode("AccountPasswordNull");
            });
        }

        private bool ContainOnlyAllowableSymbols(string property)
        {
            var allowableSymbols = string.Empty;
            if (this.accountSettingsCollection != null)
            {
                allowableSymbols = this.accountSettingsCollection["allowableSymbols"];
            }
            else
            {
                allowableSymbols = this.accountSettings.AllowableSymbols;
            }

            return property.All(symbol => allowableSymbols.Contains(char.ToLower(symbol)));
        }
    }
}
