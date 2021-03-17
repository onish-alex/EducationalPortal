namespace EducationPortal.MVC.Models
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Results;

    public class CourseStudyViewModel
    {
        public long CourseId { get; set; }

        public GetMaterialPageResult PageData { get; set; }

        public MaterialDTO ChosenMaterial { get; set; }

        public bool IsCompleted { get; set; }
    }
}
