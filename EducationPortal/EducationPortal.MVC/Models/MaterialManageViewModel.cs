namespace EducationPortal.MVC.Models
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Utilities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MaterialManageViewModel
    {
        public long CourseId { get; set; }

        public MaterialDTO MaterialToCreate { get; set; }

        public PaginatedList<MaterialDTO> GlobalMaterials { get; set; }

        public PaginatedList<MaterialDTO> CourseMaterials { get; set; }

        public IEnumerable<SelectListItem> MaterialTypeNames { get; set; }
    }
}
