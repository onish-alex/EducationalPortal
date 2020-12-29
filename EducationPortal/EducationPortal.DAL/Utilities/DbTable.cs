using System;
using System.Linq;
using System.Collections.Generic;
using EducationalPortal.DAL.Entities;

namespace EducationalPortal.DAL.Utilities
{
    public class DbTable<T> where T : Entity
    {
        public bool IsSynch { get; set; }
        private List<T> _content;
        public IEnumerable<T> Content { get => _content.AsEnumerable(); }

        public DbTable()
        {
            _content = new List<T>();
            IsSynch = true;
        }

        public DbTable(IEnumerable<T> starterSet) : this()
        {
            if (starterSet != null)
                foreach (var item in starterSet)
                    _content.Add(item);
        }

        public void Add(T item)
        {
            var withExistedId = _content.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId == null)
            {
                _content.Add(item);
                IsSynch = false;
            }
        }

        public void Update(T item)
        {
            var withExistedId = _content.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId != null)
            {
                _content.Remove(withExistedId);
                _content.Add(item);
                IsSynch = false;
            }
        }

        public void Remove(T item)
        {
            _content.Remove(item);
            IsSynch = false;
        }

        public IEnumerable<T> Find(Func<T, bool> predicate) => _content.Where(predicate);

        public IEnumerable<T> GetAll() => _content.AsReadOnly();
    }
}
