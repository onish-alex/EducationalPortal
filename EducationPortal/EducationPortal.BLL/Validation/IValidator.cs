namespace EducationPortal.BLL.Validation
{
    using FluentValidation.Results;

    public interface IValidator<T>
    {
        public ValidationResult Validate(T model);

        public ValidationResult Validate(T model, params string[] ruleSetNames);
    }
}
