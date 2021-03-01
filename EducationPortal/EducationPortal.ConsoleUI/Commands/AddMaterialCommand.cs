namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Validation;

    public class AddMaterialCommand : ICommand<AddMaterialResponse>
    {
        private IMaterialService reciever;
        private MaterialDTO material;
        private IValidator validator;

        public AddMaterialCommand(IMaterialService reciever, IValidator validator, MaterialDTO material)
        {
            this.reciever = reciever;
            this.material = material;
            this.validator = validator;
        }

        public AddMaterialResponse Response { get; private set; }

        public void Execute()
        {
            this.validator.SetData(this.material);
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.AddMaterial(this.material)
                                                     : new AddMaterialResponse() { Message = validationResult.Message };
        }
    }
}
