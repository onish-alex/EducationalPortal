using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private IRepository<Role> baseRepository;

        public RoleRepository(IRepository<Role> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public void Create(Role item)
        {
            baseRepository.Create(item);
        }

        public void Delete(long id)
        {
            baseRepository.Delete(id);
        }

        public IEnumerable<Role> Find(Func<Role, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public IEnumerable<Role> GetAll()
        {
            return baseRepository.GetAll();
        }

        public Role GetById(long id)
        {
            return baseRepository.GetById(id);
        }

        public void Update(Role item)
        {
            baseRepository.Update(item);
        }
    }
}
