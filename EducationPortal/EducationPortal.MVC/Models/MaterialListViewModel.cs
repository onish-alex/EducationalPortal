namespace EducationPortal.MVC.Models
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Utilities;

    public class MaterialListViewModel
    {
        public long CourseId { get; set; }

        public PaginatedList<MaterialDTO> Materials { get; set; }

        public string ActionPath { get; set; }

        public string DomUpdateId { get; set; }

        public string DomSelectClass { get; set; }
    }
}
