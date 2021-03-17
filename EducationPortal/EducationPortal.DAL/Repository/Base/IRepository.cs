namespace EducationPortal.DAL.Repository.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(long id);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Create(T item);

        void Update(T item);

        void Delete(long id);

        void Save();

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetPage(int page, int pageSize);

        IEnumerable<T> GetPage(int page, int pageSize, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetPage(int page, int pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task CreateAsync(T item);

        Task DeleteAsync(long id);

        Task<T> GetByIdAsync(long id);

        Task SaveAsync();

        Task<long> CountAsync();

        Task<long> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
