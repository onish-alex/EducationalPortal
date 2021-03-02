namespace EducationPortal.BLL.Services
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Validation;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;

    public class MaterialService : IMaterialService
    {
        private IRepository<Material> materials;
        private IMapper mapper;
        private IValidator<MaterialDTO> materialValidator;

        public MaterialService(
            IRepository<Material> materials,
            IMapper mapper,
            IValidator<MaterialDTO> materialValidator)
        {
            this.materials = materials;
            this.mapper = mapper;
            this.materialValidator = materialValidator;
        }

        public string Name => "Material";

        public AddMaterialResult AddMaterial(MaterialDTO material)
        {
            var result = new AddMaterialResult();

            if (material == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "MaterialNull";
                return result;
            }

            var validationRuleSetStr = string.Empty;

            switch (material)
            {
                case ArticleDTO _:
                    validationRuleSetStr = "Article";
                    break;

                case BookDTO _:
                    validationRuleSetStr = "Book";
                    break;

                case VideoDTO _:
                    validationRuleSetStr = "Video";
                    break;
            }

            var validationResult = this.materialValidator.Validate(material, "Common", validationRuleSetStr);

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var materialToAdd = this.mapper.Map<MaterialDTO, Material>(material);

            this.materials.Create(materialToAdd);
            this.materials.Save();

            result.MaterialId = materialToAdd.Id;
            result.IsSuccessful = true;

            return result;
        }

        public GetMaterialsResult GetAllMaterials()
        {
            var result = new GetMaterialsResult();

            var allMaterials = this.materials.GetAll();

            var allMaterialDTOs = new List<MaterialDTO>();

            foreach (var item in allMaterials)
            {
                var materialToAdd = this.mapper.Map<Material, MaterialDTO>(item);
                allMaterialDTOs.Add(materialToAdd);
            }

            result.IsSuccessful = allMaterialDTOs.Count != 0;

            if (!result.IsSuccessful)
            {
                result.MessageCode = "GetAllMaterialsEmptyResult";
            }

            result.Materials = allMaterialDTOs;

            return result;
        }

        public OperationResult CheckMaterialExisting(long materialId)
        {
            var result = new OperationResult();

            if (this.materials.GetById(materialId) == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CheckMaterialExistingNotFound";
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
