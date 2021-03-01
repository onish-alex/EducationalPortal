namespace EducationPortal.BLL.Services
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;

    public interface IMaterialService : IService
    {
        AddMaterialResponse AddMaterial(MaterialDTO material);

        GetMaterialsResponse GetAllMaterials();

        OperationResponse CheckMaterialExisting(long materialId);
    }
}
