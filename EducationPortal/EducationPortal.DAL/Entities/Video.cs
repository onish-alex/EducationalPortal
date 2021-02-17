using Newtonsoft.Json;
using EducationPortal.DAL.Utilities;

namespace EducationPortal.DAL.Entities
{
    [JsonConverter(typeof(MaterialConverter))]
    public class Video : Material
    {
        public int Duration { get; set; }

        public string Quality { get; set; }
    }
}
