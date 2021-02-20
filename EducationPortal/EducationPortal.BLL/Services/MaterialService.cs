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

        public AddMaterialResponse AddMaterial(MaterialDTO material)
        {
            var response = new AddMaterialResponse();

            var materialToAdd = this.mapper.Map<MaterialDTO, Material>(material);

            this.materials.Create(materialToAdd);
            this.materials.Save();

            response.MaterialId = materialToAdd.Id;
            response.IsSuccessful = true;

            return response;
        }

        public GetMaterialsResponse GetAllMaterials()
        {
            var response = new GetMaterialsResponse();

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
                response.Message = ResponseMessages.GetAllMaterialsEmptyResult;
            }

            response.Materials = allMaterialDTOs;

            return response;
        }

        public OperationResponse CheckMaterialExisting(long materialId)
        {
            var response = new OperationResponse();

            if (this.materials.GetById(materialId) == null)
            {
                response.IsSuccessful = false;
                response.Message = ResponseMessages.CheckMaterialExistingNotFound;
            }

            response.IsSuccessful = true;

            return response;
        }
    }
}
