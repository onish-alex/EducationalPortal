namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Validation;

    public class RegisterCommand : ICommand<OperationResponse>
    {
        private IUserService reciever;
        private UserDTO user;
        private AccountDTO account;
        private IValidator validator;

        public RegisterCommand(IUserService reciever, IValidator validator, UserDTO user, AccountDTO account)
        {
            this.reciever = reciever;
            this.user = user;
            this.account = account;
            this.validator = validator;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.validator.SetData(this.user, this.account);
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.Register(this.user, this.account)
                                                     : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
