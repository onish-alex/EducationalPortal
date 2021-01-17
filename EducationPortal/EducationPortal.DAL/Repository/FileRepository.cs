using System;
using System.Linq;
using System.Collections.Generic;
using EducationPortal.DAL.DbContexts;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Repository
{
    public class FileRepository<T> : IRepository<T> where T : Entity
    {
        protected FileDbContext db;
        public FileRepository(FileDbContext context)
        {
            db = context;
        }

        public void Create(T item) => db.GetTable<T>().Add(item);

        public void Delete(long id)
        {
            var entities = db.GetTable<T>();
            var entityToRemove = entities.Find(a => a.Id == id)
                                         .SingleOrDefault();
            if (entityToRemove != null)
                entities.Remove(entityToRemove);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate) => GetAll().Where(predicate);

        public IEnumerable<T> GetAll() => db.GetTable<T>()
                                             .Content
                                             .Select(a => (T)a.Clone());

        public T GetById(long id)
        {
            var entities = db.GetTable<T>();
            var entityWithId = entities
                                .Find(a => a.Id == id)
                                .SingleOrDefault();
            if (entityWithId != null)
                return entityWithId.Clone() as T;

            return null;
        }

        public void Update(T item)
        {
            db.GetTable<T>()
               .Update(item);
        }

        public void Save()
        {
            db.Save<T>();
        }
    }
}
