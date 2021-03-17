namespace EducationPortal.BLL.Results
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Results;

    public class GetSingleCourseResult : OperationResult
    {
        public CourseDTO Course { get; set; }
    }
}
