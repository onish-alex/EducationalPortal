namespace EducationPortal.BLL.DTO
{
    public class BookDTO : MaterialDTO
    {
        public string AuthorNames { get; set; }

        public string PageCount { get; set; }

        public string Format { get; set; }

        public string PublishingYear { get; set; }
    }
}
