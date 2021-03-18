namespace EducationPortal.MVC.Models
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Utilities;

    public class CourseStudyApiViewModel
    {
        public long CourseId { get; set; }

        public bool IsCompleted { get; set; }

        public PaginatedList<MaterialDTO> Materials { get; set; }

        public IDictionary<long, bool> MaterialCompleteness { get; set; }
    }
}
