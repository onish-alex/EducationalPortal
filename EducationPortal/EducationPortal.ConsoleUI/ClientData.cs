namespace EducationPortal.ConsoleUI
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class ClientData
    {
        public ClientData()
        {
            this.InputBuffer = new List<string>();
        }

        public bool IsAuthorized { get; set; }

        public long Id { get; set; }

        public UserDTO Info { get; set; }

        public CourseDTO[] CourseCache { get; set; }

        public List<string> InputBuffer { get; set; }

        public string ConsoleStatePrefix { get; set; }

        public CourseDTO SelectedCourse { get; set; }
    }
}
