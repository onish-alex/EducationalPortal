namespace EducationPortal.BLL.Validation
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using FluentValidation;

    public class UserValidator : AbstractValidator<UserDTO>
    {
        private NameValueCollection userSettings;

        public UserValidator()
        {
            this.userSettings = ConfigurationManager.GetSection("accountSettings") as NameValueCollection;

            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("UserNameLength");

            this.RuleFor(x => x.Name.Length)
                .LessThanOrEqualTo(int.Parse(this.userSettings["NameMaxLength"]))
                .WithErrorCode("UserNameLength");

            this.RuleFor(x => x.Name.Length)
                .GreaterThanOrEqualTo(int.Parse(this.userSettings["NameMinLength"]))
                .WithErrorCode("UserNameLength");

            this.RuleFor(x => x.Name)
                .Must(this.ContainOnlyAllowableSymbols)
                .WithErrorCode("AccountLoginUnallowableSymbols");
        }

        private bool ContainOnlyAllowableSymbols(string property)
        {
            var allowableSymbols = this.userSettings["allowableSymbols"];
            return property.All(symbol => allowableSymbols.Contains(symbol));
        }
    }
}
