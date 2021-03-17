namespace EducationPortal.BLL.Services
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;

    public interface IUserService : IService
    {
        OperationResult Register(UserDTO user, AccountDTO account);

        AuthorizeResult Authorize(AccountDTO account);

        GetUserInfoResult GetUserById(long userId);

        OperationResult JoinToCourse(long userId, long courseId);

        CompletedCourseResult AddCompletedCourse(long userId, long courseId);

        GetCoursesResult GetJoinedCourses(long userId);

        GetCoursesResult GetCompletedCourses(long userId);

        GetMaterialsResult GetNextMaterial(long userId, long courseId);
    }
}
