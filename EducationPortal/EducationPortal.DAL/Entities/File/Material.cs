namespace EducationPortal.DAL.Entities.File
{
    using EducationPortal.DAL.Utilities;
    using Newtonsoft.Json;

    [JsonConverter(typeof(MaterialConverter))]
    public class Material : Entity
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string MaterialType { get; set; }
    }
}
