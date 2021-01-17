using System;
using System.Collections.Generic;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Repository
{
    public interface IRepository<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(long id);
        void Save();
    }
}
