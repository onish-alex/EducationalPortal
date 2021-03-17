namespace EducationPortal.MVC.Models
{
    using EducationPortal.BLL.DTO;

    public class AddMaterialViewModel
    {
        public MaterialDTO Material { get; set; }

        public long CourseId { get; set; }
    }
}
