using System;
using System.Linq;
using System.Collections.Generic;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Utilities
{
    public class DbTable : IDbTable
    {
        private Dictionary<Entity, EntityState> content;
        public IEnumerable<Entity> Content {
            get 
            {
                return content
                        .Keys
                        .Where(a => content[a] != EntityState.Deleted)
                        .AsEnumerable();
            } 
        }

        public IEnumerable<KeyValuePair<Entity, EntityState>> ContentWithState { get => content.AsEnumerable(); }

        public DbTable()
        {
            content = new Dictionary<Entity, EntityState>();
        }

        public DbTable(IEnumerable<Entity> starterSet) : this()
        {
            if (starterSet != null)
            {
                foreach (var item in starterSet)
                {
                    content.Add(item, EntityState.Synchronized);
                }
            }
        }

        public Entity GetById(long id)
        {
            return content.Keys.SingleOrDefault(a => a.Id == id);
        }

        public EntityState GetEntityState(Entity item)
        {
            var withExistedId = GetById(item.Id);
            
            if (withExistedId != null)
            {
                return content[withExistedId];
            }
            
            return EntityState.NotFound;
        }

        public void SetEntityState(Entity item, EntityState state)
        {
            var withExistedId = GetById(item.Id);

            if (withExistedId != null)
            {
                content[withExistedId] = state;
            }
        }

        public void Add(Entity item)
        {
            //var withExistedId = GetById(item.Id);

            //if (withExistedId == null)
            //{
                content.Add(item, EntityState.Created);
            //}
        }

        public void Update(Entity item)
        {
            var withExistedId = GetById(item.Id);

            if (withExistedId != null)
            {
                content.Remove(withExistedId);
                content.Add(item, EntityState.Updated);
            }
        }

        public void Remove(Entity item)
        {
            var withExistedId = GetById(item.Id);

            if (withExistedId != null)
            {
                content[withExistedId] = EntityState.Deleted;
            }
        }

        public void RemoveFromTable(Entity item)
        {
            var withExistedId = GetById(item.Id);

            if (withExistedId != null)
            {
                content.Remove(withExistedId);
            }
        }

        public IEnumerable<Entity> Find(Func<Entity, bool> predicate) => content.Keys.Where(predicate);
    }
}
