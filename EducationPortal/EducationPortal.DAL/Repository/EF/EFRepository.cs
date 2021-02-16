namespace EducationPortal.DAL.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Repository.Base;
    using Microsoft.EntityFrameworkCore;

    public abstract class EFRepository<T> : IRepository<T>
        where T : class
    {
        protected DbContext db;

        public EFRepository(DbContext db)
        {
            this.db = db;
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
        }

        protected abstract IQueryable<T> SelectQuery { get; }

        public void Create(T item)
        {
            this.db.Set<T>().Add(item);
        }

        public void Delete(long id)
        {
            var table = this.db.Set<T>();
            var itemToDelete = table.Find(id);
            table.Remove(itemToDelete);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.SelectQuery.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return this.SelectQuery;
        }

        public abstract T GetById(long id);

        public void Save()
        {
            this.db.SaveChanges();
        }

        public virtual void Update(T item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}
