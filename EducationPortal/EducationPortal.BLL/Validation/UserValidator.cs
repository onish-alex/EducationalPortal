namespace EducationPortal.BLL.Validation
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using FluentValidation;
    using FluentValidation.Results;

    public class UserValidator : AbstractValidator<UserDTO>, IValidator<UserDTO>
    {
        private NameValueCollection userSettings;

        public UserValidator()
        {
            this.userSettings = ConfigurationManager.GetSection("accountSettings") as NameValueCollection;

            this.RuleFor(x => x.Name)
                .NotNull()
                .WithErrorCode("UserNameLength")
                .NotEmpty()
                .WithErrorCode("UserNameLength")
                .Must(this.ContainOnlyAllowableSymbols)
                .WithErrorCode("AccountLoginUnallowableSymbols");

            this.RuleFor(x => x.Name.Length)
                .LessThanOrEqualTo(int.Parse(this.userSettings["NameMaxLength"]))
                .WithErrorCode("UserNameLength")
                .GreaterThanOrEqualTo(int.Parse(this.userSettings["NameMinLength"]))
                .WithErrorCode("UserNameLength");
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
            var allowableSymbols = this.userSettings["allowableSymbols"];
            return property.All(symbol => allowableSymbols.Contains(char.ToLower(symbol)));
        }
    }
}
