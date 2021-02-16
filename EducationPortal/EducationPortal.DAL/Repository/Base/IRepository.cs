namespace EducationPortal.DAL.Repository.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(long id);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Create(T item);

        void Update(T item);

        void Delete(long id);

        void Save();
    }
}
