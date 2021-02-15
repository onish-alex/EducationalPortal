﻿using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Utilities
{
    class MaterialConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObj = JObject.Load(reader);
            var type = jObj.Value<string>("MaterialType");
            var properties = jObj.Properties().ToDictionary(key => key.Name, value => value.Value);

            switch (type)
            {
                case "Book":
                    return new Book()
                    {
                        Id = (long)properties["Id"],
                        Format = (string)properties["Format"],
                        AuthorNames = properties["AuthorNames"].ToObject<string[]>(),
                        MaterialType = (string)properties["MaterialType"],
                        Name = (string)properties["Name"],
                        PageCount = (int)properties["PageCount"],
                        PublishingYear = (int)properties["PublishingYear"],
                        Url = (string)properties["Url"]
                    };


                case "Article":
                    return new Article()
                    {
                        Id = (long)properties["Id"],
                        MaterialType = (string)properties["MaterialType"],
                        Name = (string)properties["Name"],
                        Url = (string)properties["Url"],
                        PublicationDate = (DateTime)properties["PublicationDate"]
                        //PublicationDate = DateTime.ParseExact(properties["PublicationDate"].ToString(), "yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture)
                    };

                case "Video":
                    return new Video()
                    {
                        Id = (long)properties["Id"],
                        MaterialType = (string)properties["MaterialType"],
                        Name = (string)properties["Name"],
                        Url = (string)properties["Url"],
                        Duration = (int)properties["Duration"],
                        Quality = (string)properties["Quality"]
                    };

                default: throw new Exception("Wrong material type");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jObj = new JObject();
            Type type = value.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (prop.CanRead)
                {
                    object propVal = prop.GetValue(value, null);
                    if (propVal != null)
                    {
                        jObj.Add(prop.Name, JToken.FromObject(propVal, serializer));
                    }
                }
            }
            jObj.WriteTo(writer);
        }
    }
}
