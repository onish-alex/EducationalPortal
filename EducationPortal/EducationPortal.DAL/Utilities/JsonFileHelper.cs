using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using EducationPortal.DAL.Entities;
using System.Linq;
using System.Threading;

namespace EducationPortal.DAL.Utilities
{
    public class JsonFileHelper
    {
        private JsonSerializer serializer;
        private static readonly string extension = ".json";
        private static readonly JsonFileHelper instance = new JsonFileHelper();
        private Mutex mutex;

        private JsonFileHelper()
        {
            this.serializer = new JsonSerializer();
            this.mutex = new Mutex(false, @"Global/EducationPortal");
        }

        public static JsonFileHelper GetInstance()
        {
            return instance;
        }

        public void SaveTable(string tablePath, IDictionary<Entity, EntityState> content)
        {
            mutex.WaitOne();
            var hasGotNextId = long.TryParse(File.ReadAllText(tablePath + DbConfig.dbIdsFileName), out long nextId);
            mutex.ReleaseMutex();

            if (!hasGotNextId)
            {
                nextId = 1;
            }

            foreach (var item in content)
            {
                if (item.Value == EntityState.Created)
                {
                    item.Key.Id = nextId++;
                }

                var fileName = tablePath + item.Key.Id + extension;
                
                if (item.Value == EntityState.Deleted)
                {
                    File.Delete(fileName);
                    continue;
                }

                var fileInfo = new FileInfo(fileName);
                mutex.WaitOne();

                using (StreamWriter sw = fileInfo.CreateText())
                {
                    using (JsonWriter jw = new JsonTextWriter(sw))
                    {

                        serializer.Serialize(jw, item.Key);
                    }
                }
                mutex.ReleaseMutex();
                
            }

            mutex.WaitOne();
            File.WriteAllText(tablePath + DbConfig.dbIdsFileName, nextId.ToString());
            mutex.ReleaseMutex();
        }

        public IEnumerable<T> ReadTable<T>(string tablePath) where T : Entity
        {
            var entityPaths = Directory.EnumerateFiles(tablePath).Where(file => Path.GetRelativePath(tablePath, file) != DbConfig.dbIdsFileName);
            var tableContent = new List<T>();
            mutex.WaitOne();
            foreach (var entity in entityPaths)
            {
                using (StreamReader sr = new StreamReader(entity))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        tableContent.Add(serializer.Deserialize<T>(jr));
                    }
                }
            }
            mutex.ReleaseMutex();


            return tableContent;
        }

        public T ReadEntity<T>(string tablePath, long id) where T : Entity
        {
            var fileName = tablePath + id + extension;
            T entity;

            mutex.WaitOne();
            
            using (StreamReader sr = new StreamReader(fileName))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    entity = serializer.Deserialize<T>(jr);
                }
            }

            mutex.ReleaseMutex();

            return entity;
        }
    }
}
