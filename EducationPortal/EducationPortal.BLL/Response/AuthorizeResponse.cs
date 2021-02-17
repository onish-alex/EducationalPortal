using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Response
{
    public class AuthorizeResponse : OperationResponse
    {
        public long Id { get; set; }

        public UserDTO User { get; set; }
    }
}
