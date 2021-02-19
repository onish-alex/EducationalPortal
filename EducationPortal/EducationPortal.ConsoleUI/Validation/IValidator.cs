namespace EducationPortal.ConsoleUI.Validation
{
    public interface IValidator
    {
        ValidationResult Validate();

        string Name { get; }

        void SetData(params object[] dtos);
    }
}
