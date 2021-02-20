namespace EducationPortal.ConsoleUI.Validation
{
    public interface IValidator
    {
        string Name { get; }

        ValidationResult Validate();

        void SetData(params object[] dtos);
    }
}
