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
        private MaterialDataValidator validator;

        public AddMaterialCommand(IMaterialService reciever, MaterialDTO material)
        {
            this.reciever = reciever;
            this.material = material;
            this.validator = new MaterialDataValidator(material);
        }

        public AddMaterialResponse Response { get; private set; }

        public void Execute()
        {
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.AddMaterial(this.material)
                                                     : new AddMaterialResponse() { Message = validationResult.Message };
        }
    }
}
