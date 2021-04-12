namespace EducationPortal.MVC.Models
{
    using System.Collections.Generic;
    using EducationPortal.BLL.Results;

    public class ConcreteCourseViewModel
    {
        public long Id { get; set; }

        public GetCourseStatusResult CourseInfo { get; set; }

        public IEnumerable<string> DetailInfo { get; set; }
    }
}
