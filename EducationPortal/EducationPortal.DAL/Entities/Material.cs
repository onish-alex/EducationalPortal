using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using EducationPortal.DAL.Utilities;

namespace EducationPortal.DAL.Entities
{
    [JsonConverter(typeof(MaterialConverter))]
    public class Material : Entity
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string MaterialType { get; set; }
    }
}
