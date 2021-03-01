namespace EducationPortal.ConsoleUI.Validation
{
    using System.Linq;

    public abstract class Validator : IValidator
    {
        public abstract string Name { get; }

        public abstract ValidationResult Validate();

        public void SetData(params object[] dtos)
        {
            var props = this.GetType().GetProperties();

            foreach (var dto in dtos)
            {
                var prop = props.FirstOrDefault(x => x.PropertyType.Name == dto.GetType().Name
                                                  || x.PropertyType.Name == dto.GetType().BaseType.Name);
                prop.SetValue(this, dto);
            }
        }
    }
}
