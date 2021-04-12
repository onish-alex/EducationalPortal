namespace EducationPortal.MVC.Models
{
    using EducationPortal.BLL.DTO;

    public class RegisterViewModel
    {
        public UserDTO User { get; set; }

        public AccountDTO Account { get; set; }
    }
}
