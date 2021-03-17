namespace EducationPortal.MVC.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Utilities;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;

    public class AccountController : Controller
    {
        private IUserService userService;
        private IResourceHelper resourceHelper;
        private IValidator<AccountDTO> accountValidator;
        private IValidator<UserDTO> userValidator;
        private ISignInManager signInManager;

        public AccountController(
            IUserService userService,
            IResourceHelper resourceHelper,
            IValidator<AccountDTO> accountValidator,
            IValidator<UserDTO> userValidator,
            ISignInManager signInManager)
        {
            this.userService = userService;
            this.resourceHelper = resourceHelper;
            this.accountValidator = accountValidator;
            this.userValidator = userValidator;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountDTO model)
        {
            model.Email = model.Login;

            var validationResult = this.accountValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(
                    string.Empty,
                    this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var result = this.userService.Authorize(model);
                if (result.IsSuccessful)
                {
                    await this.signInManager.SignInAsync(model.Login, result.Id);
                    return this.RedirectToAction("Global", "Section");
                }

                this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(result.MessageCode));
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountDTO account, UserDTO user)
        {
            var validationResult = this.accountValidator.Validate(account, opt => opt.IncludeRuleSets("Detail"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(
                    string.Empty,
                    this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }
            else
            {
                validationResult = this.userValidator.Validate(user);

                if (!validationResult.IsValid)
                {
                    this.ModelState.AddModelError(
                        string.Empty,
                        this.resourceHelper.GetValidationErrorString(validationResult.Errors[0].ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var result = await this.userService.Register(user, account);
                if (result.IsSuccessful)
                {
                    return this.RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(result.MessageCode));
                }
            }

            return this.View(new RegisterViewModel()
            {
                Account = account,
                User = user,
            });
        }

        [HttpGet]
        public IActionResult Cabinet()
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = this.userService.GetUserById(userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Course", new { message = message });
            }

            return this.View(result);
        }

        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Login", "Account");
        }
    }
}
