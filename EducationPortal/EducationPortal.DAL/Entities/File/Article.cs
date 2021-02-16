namespace EducationPortal.DAL.Entities.File
{
    using System;
    using EducationPortal.DAL.Utilities;
    using Newtonsoft.Json;

    [JsonConverter(typeof(MaterialConverter))]
    public class Article : Material
    {
        public DateTime PublicationDate { get; set; }
    }
}
