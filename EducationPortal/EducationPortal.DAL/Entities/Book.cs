using Newtonsoft.Json;
using EducationPortal.DAL.Utilities;

namespace EducationPortal.DAL.Entities
{
    [JsonConverter(typeof(MaterialConverter))]
    public class Book : Material
    {
        public string[] AuthorNames { get; set; }

        public int PageCount { get; set; } 

        public string Format { get; set; }

        public int PublishingYear { get; set; }
    }
}
