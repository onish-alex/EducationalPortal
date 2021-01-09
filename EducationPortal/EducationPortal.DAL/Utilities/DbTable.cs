using System;
using System.Linq;
using System.Collections.Generic;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Utilities
{

    public class DbTable<T> : IDbTable<T> where T : Entity
    {
        private Dictionary<T, EntityState> _content;
        public IEnumerable<T> Content { get => _content.Keys.Where(a => _content[a] != EntityState.Deleted).AsEnumerable(); }

        public IEnumerable<KeyValuePair<T, EntityState>> ContentWithState { get => _content.AsEnumerable();  }

        public DbTable()
        {
            _content = new Dictionary<T, EntityState>();
        }

        public DbTable(IEnumerable<T> starterSet) : this()
        {
            if (starterSet != null)
                foreach (var item in starterSet)
                    _content.Add(item, EntityState.Synchronized);
        }

        public EntityState GetEntityState(T item)
        {
            var withExistedId = _content.Keys.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId != null)
            {
                return _content[withExistedId];
            }
            return EntityState.NotFound;
        }

        public void SetEntityState(T item, EntityState state)
        {
            var withExistedId = _content.Keys.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId != null)
            {
                _content[withExistedId] = state;
            }
        }

        public void Add(T item)
        {
            var withExistedId = _content.Keys.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId == null)
            {
                _content.Add(item, EntityState.Created);
            }
        }

        public void Update(T item)
        {
            var withExistedId = _content.Keys.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId != null)
            {
                _content.Remove(withExistedId);
                _content.Add(item, EntityState.Updated);
            }
        }

        public void Remove(T item)
        {
            var withExistedId = _content.Keys.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId != null)
            {
                _content[withExistedId] = EntityState.Deleted;
            }
        }

        public void RemoveFromTable(T item)
        {
            var withExistedId = _content.Keys.SingleOrDefault(a => a.Id == item.Id);
            if (withExistedId != null)
            {
                _content.Remove(withExistedId);
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate) => _content.Keys.Where(predicate);
    }
}
