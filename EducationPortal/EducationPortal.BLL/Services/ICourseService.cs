namespace EducationPortal.BLL.Services
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;

    public interface ICourseService : IService
    {
        OperationResponse AddCourse(CourseDTO course);

        GetCoursesResponse GetUserCourses(long userId);

        GetCoursesResponse GetAllCourses();

        OperationResponse EditCourse(long userId, CourseDTO newCourseInfo);

        OperationResponse AddSkill(long userId, long courseId, SkillDTO skill);

        OperationResponse RemoveSkill(long userId, long courseId, SkillDTO skill);

        OperationResponse AddMaterialToCourse(long userId, long courseId, long materialId);

        OperationResponse CanEditCourse(long userId, long courseId);

        OperationResponse CanJoinCourse(long userId, long courseId);

        GetCourseStatusResponse GetCourseStatus(long courseId, long userId);
    }
}
