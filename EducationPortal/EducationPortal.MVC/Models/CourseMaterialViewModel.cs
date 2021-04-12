namespace EducationPortal.MVC.Models
{
    using EducationPortal.BLL.DTO;

    public class CourseMaterialViewModel
    {
        public MaterialDTO Material { get; set; }

        public bool IsLearned { get; set; }

        public long CourseId { get; set; }
    }
}
