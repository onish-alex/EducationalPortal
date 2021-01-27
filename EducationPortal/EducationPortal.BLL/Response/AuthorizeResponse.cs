using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Response
{
    public class AuthorizeResponse : IResponse
    {
        public long Id { get; set; }

        public UserDTO User { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
