using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using EducationPortal.ConsoleUI.Validation;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AddMaterialCommand : ICommand<AddMaterialResponse>
    {
        public AddMaterialResponse Response { get; private set; }

        private IMaterialService reciever;
        private MaterialDTO material;
        private MaterialDataValidator validator;

        public AddMaterialCommand(IMaterialService reciever, MaterialDTO material)
        {
            this.reciever = reciever;
            this.material = material;
            this.validator = new MaterialDataValidator(material);
        }

        public void Execute()
        {
            var validationResult = validator.Validate();
            Response = (validationResult.IsValid) ? reciever.AddMaterial(material)
                                                  : new AddMaterialResponse() { Message = validationResult.Message };
        }
    }
}
