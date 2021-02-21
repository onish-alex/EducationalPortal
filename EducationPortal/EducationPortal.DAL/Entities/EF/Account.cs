namespace EducationPortal.DAL.Entities.EF
{
    public class Account
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public User User { get; set; }
    }
}
