namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL.DTO;

    public class Client
    {
        private static Client instance = new Client();

        private Client()
        {
        }

        public long Id { get; set; }

        public UserDTO Info { get; set; }

        public CourseDTO[] CourseCache { get; set; }

        public static Client GetInstance()
        {
            return instance;
        }
    }
}
