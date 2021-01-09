using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Utilities
{
    public class JsonFileHelper
    {
        private JsonSerializer _serializer;
        private static readonly string extension = ".json";
        private static readonly JsonFileHelper instance = new JsonFileHelper();

        private JsonFileHelper()
        {
            _serializer = new JsonSerializer();
        }

        public static JsonFileHelper GetInstance()
        {
            return instance;
        }

        public void SaveTable(string tablePath, IDictionary<Entity, EntityState> content)
        {
            foreach (var item in content)
            {
                var fileName = tablePath + item.Key.Id + extension;

                if (item.Value == EntityState.Deleted)
                {
                    File.Delete(fileName);
                    continue;
                }

                var fileInfo = new FileInfo(fileName);

                using (StreamWriter sw = fileInfo.CreateText())
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    _serializer.Serialize(jw, item.Key);
                }
            }
        }

        public IEnumerable<T> ReadTable<T>(string tablePath) where T : Entity
        {
            var entityPaths = Directory.EnumerateFiles(tablePath);

            var tableContent = new List<T>();
            foreach (var entity in entityPaths)
                using (StreamReader sr = new StreamReader(entity))
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    tableContent.Add(_serializer.Deserialize<T>(jr));
                }
            
            return tableContent;
        }

        public T ReadEntity<T>(string tablePath, long id) where T : Entity
        {
            var fileName = tablePath + id + extension;

            T entity;

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                entity = _serializer.Deserialize<T>(jr);
            }

            return entity;
        }
    }
}
