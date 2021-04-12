namespace EducationPortal.BLL.Results
{
    using EducationPortal.BLL.DTO;

    public class AuthorizeResult : OperationResult
    {
        public long Id { get; set; }

        public UserDTO User { get; set; }
    }
}
