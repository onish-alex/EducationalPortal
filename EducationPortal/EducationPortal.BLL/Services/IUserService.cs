using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;

namespace EducationPortal.BLL.Services
{
    public interface IUserService : IService
    {
        OperationResponse Register(UserDTO user, AccountDTO account);

        AuthorizeResponse Authorize(AccountDTO account);

        GetUserResponse GetUserById(long userId);

        OperationResponse JoinToCourse(long userId, long courseId);

        OperationResponse AddLearnedMaterial(long userId, long materialId);

        OperationResponse AddCompletedCourse(long userId, CourseDTO course);

    }
}
