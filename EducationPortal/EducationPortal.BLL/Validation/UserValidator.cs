namespace EducationPortal.BLL.Validation
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class UserValidator : AbstractValidator<UserDTO>, IValidator<UserDTO>
    {
        private NameValueCollection userSettingsCollection;
        private AccountSettings userSettings;

        public UserValidator()
        {
            this.userSettingsCollection = ConfigurationManager.GetSection("accountSettings") as NameValueCollection;

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode("UserNameLength")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Name)
                        .Must(this.ContainOnlyAllowableSymbols)
                        .WithErrorCode("AccountLoginUnallowableSymbols");

                    this.RuleFor(x => x.Name.Length)
                        .LessThanOrEqualTo(int.Parse(this.userSettingsCollection["NameMaxLength"]))
                        .WithErrorCode("UserNameLength")
                        .GreaterThanOrEqualTo(int.Parse(this.userSettingsCollection["NameMinLength"]))
                        .WithErrorCode("UserNameLength");
                });
        }

        public UserValidator(IOptionsSnapshot<AccountSettings> options)
        {
            this.userSettings = options.Value;

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode("UserNameLength")
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Name)
                        .Must(this.ContainOnlyAllowableSymbols)
                        .WithErrorCode("AccountLoginUnallowableSymbols");

                    this.RuleFor(x => x.Name.Length)
                        .LessThanOrEqualTo(this.userSettings.NameMaxLength)
                        .WithErrorCode("UserNameLength")
                        .GreaterThanOrEqualTo(this.userSettings.NameMinLength)
                        .WithErrorCode("UserNameLength");
                });
        }

        ValidationResult IValidator<UserDTO>.Validate(UserDTO model)
        {
            return this.Validate(model);
        }

        ValidationResult IValidator<UserDTO>.Validate(UserDTO model, params string[] ruleSetNames)
        {
            return this.Validate(model, opt => opt.IncludeRuleSets(ruleSetNames));
        }

        private bool ContainOnlyAllowableSymbols(string property)
        {
            var allowableSymbols = string.Empty;
            if (this.userSettingsCollection != null)
            {
                allowableSymbols = this.userSettingsCollection["allowableSymbols"];
            }
            else
            {
                allowableSymbols = this.userSettings.AllowableSymbols;
            }

            return property.All(symbol => allowableSymbols.Contains(char.ToLower(symbol)));
        }
    }
}
