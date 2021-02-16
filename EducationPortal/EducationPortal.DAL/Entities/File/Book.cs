namespace EducationPortal.DAL.Entities.File
{
    using EducationPortal.DAL.Utilities;
    using Newtonsoft.Json;

    [JsonConverter(typeof(MaterialConverter))]
    public class Book : Material
    {
        public string[] AuthorNames { get; set; }

        public int PageCount { get; set; }

        public string Format { get; set; }

        public int PublishingYear { get; set; }
    }
}
