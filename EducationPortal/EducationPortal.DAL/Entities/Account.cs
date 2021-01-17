namespace EducationPortal.DAL.Entities
{
    public class Account : Entity
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
