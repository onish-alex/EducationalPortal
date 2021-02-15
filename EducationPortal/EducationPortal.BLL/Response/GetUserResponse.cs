using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Response
{
    public class GetUserResponse : OperationResponse
    {
        public UserDTO User { get; set; }
    }
}
