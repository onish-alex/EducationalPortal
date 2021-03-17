namespace EducationPortal.BLL.Services
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;

    public interface IMaterialService : IService
    {
        AddMaterialResult AddMaterial(MaterialDTO material);

        GetMaterialsResult GetAllMaterials();

        OperationResult CheckMaterialExisting(long materialId);
    }
}
