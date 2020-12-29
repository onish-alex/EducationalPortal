using EducationalPortal.DAL.Entities;

namespace EducationalPortal.DAL.Repository
{
    public class UserRepository : RepositoryDecorator<User>
    {
        public UserRepository(IRepository<User> db) : base(db) { }
    }
}
