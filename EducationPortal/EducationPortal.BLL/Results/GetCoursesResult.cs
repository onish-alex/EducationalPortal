namespace EducationPortal.BLL.Results
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetCoursesResult : OperationResult
    {
        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}
