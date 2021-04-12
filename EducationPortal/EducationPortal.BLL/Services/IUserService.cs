namespace EducationPortal.BLL.Services
{
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Results;

    public interface IUserService
    {
        Task<OperationResult> Register(UserDTO user, AccountDTO account);

        AuthorizeResult Authorize(AccountDTO account);

        GetUserInfoResult GetUserById(long userId);

        Task<OperationResult> JoinToCourse(long userId, long courseId);

        Task<CompletedCourseResult> AddCompletedCourse(long userId, long courseId);

        GetCoursesResult GetJoinedCourses(long userId);

        GetCoursesResult GetCompletedCourses(long userId);

        Task<GetMaterialsResult> GetNextMaterial(long userId, long courseId);

        Task<OperationResult> LearnMaterial(long userId, long courseId, long materialId);
    }
}
