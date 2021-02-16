namespace EducationPortal.BLL.Services
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;

    public interface IUserService : IService
    {
        OperationResponse Register(UserDTO user, AccountDTO account);

        AuthorizeResponse Authorize(AccountDTO account);

        GetUserInfoResponse GetUserById(long userId);

        OperationResponse JoinToCourse(long userId, long courseId);

        CompletedCourseResponse AddCompletedCourse(long userId, long courseId);

        GetCoursesResponse GetJoinedCourses(long userId);

        GetCoursesResponse GetCompletedCourses(long userId);

        GetMaterialsResponse GetNextMaterial(int userId, int courseId);
    }
}
