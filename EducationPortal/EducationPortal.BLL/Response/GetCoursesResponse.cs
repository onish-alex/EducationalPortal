using System.Collections.Generic;
using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Response
{
    public class GetCoursesResponse : OperationResponse
    {
        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}
