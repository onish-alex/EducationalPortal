using System;
using System.Linq;
using System.Collections.Generic;
using EducationalPortal.DAL.DbContexts;
using EducationalPortal.DAL.Entities;

namespace EducationalPortal.DAL.Repository.Base
{
    public class FileRepository<T> : IRepository<T> where T : Entity
    {
        protected FileDbContext _db;
        public FileRepository(FileDbContext context)
        {
            _db = context;
        }

        public void Create(T item) => _db.GetTable<T>().Add(item);

        public void Delete(long id)
        {
            var entities = _db.GetTable<T>();
            var entityToRemove = entities.Find(a => a.id == id).SingleOrDefault();
            if (entityToRemove != null)
                entities.Remove(entityToRemove);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate) => GetAll().Where(predicate);

        public IEnumerable<T> GetAll() => _db.GetTable<T>()
                                             .Content
                                             .Select(a => (T)a.Clone());

        public T GetById(long id)
        {
            var entities = _db.GetTable<T>();
            var entityWithId = entities
                                .Find(a => a.id == id)
                                .SingleOrDefault();
            if (entityWithId != null)
                return entityWithId.Clone() as T;

            return null;
        }

        public void Update(T item)
        {
            _db.GetTable<T>()
               .Update(item);
        }
    }
}
