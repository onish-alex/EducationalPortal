namespace EducationPortal.DAL.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Repository.Base;
    using Microsoft.EntityFrameworkCore;

    public class EFRepository<T> : IRepository<T>
        where T : class
    {
        protected EFDbContext db;

        public EFRepository(EFDbContext db)
        {
            this.db = db;
        }

        public void Create(T item)
        {
            this.db.Set<T>().Add(item);
        }

        public async Task CreateAsync(T item)
        {
            await this.db.Set<T>().AddAsync(item);
        }

        public void Delete(long id)
        {
            var table = this.db.Set<T>();
            var itemToDelete = table.Find(id);
            table.Remove(itemToDelete);
        }

        public async Task DeleteAsync(long id)
        {
            var table = this.db.Set<T>();
            var itemToDelete = await table.FindAsync(id);
            table.Remove(itemToDelete);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.db.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return this.Get(includes).Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return this.db.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return this.Get(includes).ToList();
        }

        public T GetById(long id)
        {
            return this.db.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await this.db.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> GetPage(int page, int pageSize)
        {
            return this.db.Set<T>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<T> GetPage(int page, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            return this.Get(includes)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<T> GetPage(int page, int pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return this.Get(includes)
                .Where(predicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void Save()
        {
            this.db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await this.db.SaveChangesAsync();
        }

        public virtual void Update(T item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }

        public async Task<long> CountAsync()
        {
            return await this.db.Set<T>().LongCountAsync();
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.db.Set<T>().Where(predicate).LongCountAsync();
        }

        protected virtual IQueryable<T> Get(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result = this.db.Set<T>();
            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return result;
        }
    }
}
