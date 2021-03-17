namespace EducationPortal.BLL.Services
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;

    public interface ICourseService : IService
    {
        OperationResult AddCourse(CourseDTO course);

        GetCoursesResult GetUserCourses(long userId);

        GetCoursesResult GetAllCourses();

        OperationResult EditCourse(long userId, CourseDTO newCourseInfo);

        OperationResult AddSkill(long userId, long courseId, SkillDTO skill);

        OperationResult RemoveSkill(long userId, long courseId, SkillDTO skill);

        OperationResult AddMaterialToCourse(long userId, long courseId, long materialId);

        OperationResult CanEditCourse(long userId, long courseId);

        GetCourseStatusResult GetCourseStatus(long courseId, long userId);
    }
}
