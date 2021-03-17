namespace EducationPortal.BLL.Services
{
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Results;

    public interface IMaterialService
    {
        Task<AddMaterialResult> AddMaterial(MaterialDTO material);

        GetMaterialsResult GetAllMaterials();

        Task<OperationResult> CheckMaterialExisting(long materialId);

        GetMaterialPageResult GetMaterialsToStudy(long courseId, long userId, int page, int pageSize);

        Task<GetMaterialPageResult> GetCourseMaterials(long courseId, int page, int pageSize);

        Task<GetMaterialPageResult> GetGlobalMaterials(int page, int pageSize, long? courseId = null);

        Task<GetSingleMaterialResult> GetMaterial(long id);
    }
}
