using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Utilities
{
    public interface IDbTable<T> where T : Entity
    {
        public IEnumerable<T> Content { get; }

        public void Add(T item);

        public void Update(T item);

        public void Remove(T item);

        public IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
