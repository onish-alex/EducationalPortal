namespace EducationPortal.DAL.Entities.File
{
    using EducationPortal.DAL.Utilities;
    using Newtonsoft.Json;

    [JsonConverter(typeof(MaterialConverter))]
    public class Video : Material
    {
        public int Duration { get; set; }

        public string Quality { get; set; }
    }
}
