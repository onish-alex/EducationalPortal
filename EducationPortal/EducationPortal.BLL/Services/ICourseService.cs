namespace EducationPortal.BLL.Services
{
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Results;

    public interface ICourseService
    {
        Task<OperationResult> AddCourse(CourseDTO course);

        GetCoursesResult GetUserCourses(long userId);

        GetCoursesResult GetAllCourses();

        Task<OperationResult> EditCourse(long userId, CourseDTO newCourseInfo);

        Task<OperationResult> AddSkill(long userId, long courseId, SkillDTO skill);

        Task<OperationResult> RemoveSkill(long userId, long courseId, long skillId);

        Task<OperationResult> AddMaterialToCourse(long userId, long courseId, long materialId);

        Task<OperationResult> CanEditCourse(long userId, long courseId);

        GetCourseStatusResult GetCourseStatus(long courseId, long userId);

        Task<GetCoursesResult> GetGlobalCourses(int page, int pageSize);

        Task<GetCoursesResult> GetUserCourses(int page, int pageSize, long userId);

        Task<GetCoursesResult> GetJoinedCourses(int page, int pageSize, long userId);

        Task<GetCoursesResult> GetCompletedCourses(int page, int pageSize, long userId);

        GetSingleCourseResult GetCourse(long id);

        Task<OperationResult> DeleteCourse(long userId, long courseId);

        OperationResult CheckCourseCompletness(long courseId, long userId);

        Task<OperationResult> RemoveMaterialFromCourse(long userId, long courseId, long materialId);
    }
}
