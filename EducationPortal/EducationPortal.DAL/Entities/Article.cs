using System;
using Newtonsoft.Json;
using EducationPortal.DAL.Utilities;

namespace EducationPortal.DAL.Entities
{
    [JsonConverter(typeof(MaterialConverter))]
    public class Article : Material
    {
        public DateTime PublicationDate { get; set; }
    }
}
