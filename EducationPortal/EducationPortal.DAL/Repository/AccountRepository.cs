using EducationPortal.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EducationPortal.DAL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private IRepository<Account> baseRepository;

        public AccountRepository(IRepository<Account> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public void Create(Account item)
        {
            baseRepository.Create(item);
        }

        public void Delete(long id)
        {
            baseRepository.Delete(id);
        }

        public IEnumerable<Account> Find(Func<Account, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public IEnumerable<Account> GetAll()
        {
            return baseRepository.GetAll();
        }

        public Account GetById(long id)
        {
            return baseRepository.GetById(id);
        }

        public void Update(Account item)
        {
            baseRepository.Update(item);
        }
    }
}
