namespace EducationPortal.BLL.Response
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetCoursesResponse : OperationResponse
    {
        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}
