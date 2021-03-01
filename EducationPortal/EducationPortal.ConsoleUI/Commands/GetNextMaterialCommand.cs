namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class GetNextMaterialCommand : ICommand
    {
        private ClientData client;
        private IUserService userService;

        public GetNextMaterialCommand(IUserService userService, ClientData client)
        {
            this.client = client;
            this.userService = userService;
        }

        public string Name => "nextstep";

        public string Description => "nextstep\nПерейти к следующему материалу курса\n";

        public int ParamsCount => 0;

        public void Execute()
        {
            if (!this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.client.SelectedCourse == null)
            {
                Console.WriteLine(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var response = this.userService.GetNextMaterial(this.client.Id, this.client.SelectedCourse.Id);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(OperationMessages.GetString(response.MessageCode));
                return;
            }

            foreach (var item in response.Materials)
            {
                Console.WriteLine(ConsoleMessages.OutputNewMaterialLearned);
                OutputHelper.PrintSingleMaterial(item);
            }
        }
    }
}
