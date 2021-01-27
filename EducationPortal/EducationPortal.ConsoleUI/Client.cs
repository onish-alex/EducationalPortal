using EducationPortal.BLL.DTO;

namespace EducationPortal.ConsoleUI
{
    public class Client
    {
        private static Client instance = new Client();
        
        private Client()
        {

        }

        public static Client GetInstance()
        {
            return instance;
        }

        public long Id { get; set; }

        public UserDTO Info { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
