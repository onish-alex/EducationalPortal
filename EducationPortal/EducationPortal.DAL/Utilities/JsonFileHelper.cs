using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using EducationalPortal.DAL.Entities;

namespace EducationalPortal.DAL.Utilities
{
    public static class JsonFileHelper
    {
        private static JsonSerializer _serializer = new JsonSerializer();

        public static void JsonWrite(string fileName, IEnumerable<Entity> content)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                _serializer.Serialize(jw, content);
            }
        }

        public static IEnumerable<T> JsonRead<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
                return null;
            }

            IEnumerable<T> content;
            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                content = _serializer.Deserialize<IEnumerable<T>>(jr);
            }
            return content;
        }
    }
}
