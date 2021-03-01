namespace EducationPortal.BLL.Services
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Response;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;

    public class MaterialService : IMaterialService
    {
        private IRepository<Material> materials;
        private IMapper mapper;

        public MaterialService(IRepository<Material> materials, IMapper mapper)
        {
            this.materials = materials;
            this.mapper = mapper;
        }

        public string Name => "Material";

        public AddMaterialResult AddMaterial(MaterialDTO material)
        {
            var response = new AddMaterialResult();

            var materialToAdd = this.mapper.Map<MaterialDTO, Material>(material);

            this.materials.Create(materialToAdd);
            this.materials.Save();

            response.MaterialId = materialToAdd.Id;
            response.IsSuccessful = true;

            return response;
        }

        public GetMaterialsResult GetAllMaterials()
        {
            var response = new GetMaterialsResult();

            var allMaterials = this.materials.GetAll();

            var allMaterialDTOs = new List<MaterialDTO>();

            foreach (var item in allMaterials)
            {
                var materialToAdd = this.mapper.Map<Material, MaterialDTO>(item);
                allMaterialDTOs.Add(materialToAdd);
            }

            response.IsSuccessful = allMaterialDTOs.Count != 0;

            if (!response.IsSuccessful)
            {
                response.MessageCode = "GetAllMaterialsEmptyResult";
            }

            response.Materials = allMaterialDTOs;

            return response;
        }

        public OperationResult CheckMaterialExisting(long materialId)
        {
            var response = new OperationResult();

            if (this.materials.GetById(materialId) == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "CheckMaterialExistingNotFound";
            }

            response.IsSuccessful = true;

            return response;
        }
    }
}
