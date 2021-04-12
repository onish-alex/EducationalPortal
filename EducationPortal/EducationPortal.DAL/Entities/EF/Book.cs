namespace EducationPortal.DAL.Entities.EF
{
    public class Book : Material
    {
        public string AuthorNames { get; set; }

        public int PageCount { get; set; }

        public string Format { get; set; }

        public short PublishingYear { get; set; }
    }
}
