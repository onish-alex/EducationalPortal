namespace EducationPortal.MVC.Utilities
{
    using System.Threading.Tasks;

    public interface ISignInManager
    {
        public Task SignInAsync(string login, long id);

        public Task SignOutAsync();
    }
}
