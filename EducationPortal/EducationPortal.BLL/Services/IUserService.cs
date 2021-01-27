using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;

namespace EducationPortal.BLL.Services
{
    public interface IUserService : IService
    {
        RegisterResponse Register(UserDTO user, AccountDTO account);

        AuthorizeResponse Authorize(AccountDTO account);
    }
}
