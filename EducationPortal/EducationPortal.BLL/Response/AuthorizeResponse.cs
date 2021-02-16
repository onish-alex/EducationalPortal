namespace EducationPortal.BLL.Response
{
    using EducationPortal.BLL.DTO;

    public class AuthorizeResponse : OperationResponse
    {
        public int Id { get; set; }

        public UserDTO User { get; set; }
    }
}
