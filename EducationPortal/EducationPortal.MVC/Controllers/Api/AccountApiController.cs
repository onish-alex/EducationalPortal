namespace EducationPortal.MVC.Controllers.Api
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Results;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Utilities;
    using FluentValidation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private IUserService userService;
        private IResourceHelper resourceHelper;
        private IValidator<AccountDTO> accountValidator;
        private IValidator<UserDTO> userValidator;
        private ISignInManager signInManager;

        public AccountApiController(
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

        [HttpPost("Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel viewModel)
        {
            var accountDto = new AccountDTO()
            {
                Email = viewModel.Account.Email,
                Login = viewModel.Account.Login,
                Password = viewModel.Account.Password,
            };

            var userDto = new UserDTO()
            {
                Name = viewModel.User.Name,
            };

            var validationResult = this.accountValidator.Validate(accountDto, opt => opt.IncludeRuleSets("Detail"));

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
                validationResult = this.userValidator.Validate(userDto);

                if (!validationResult.IsValid)
                {
                    this.ModelState.AddModelError(
                        string.Empty,
                        this.resourceHelper.GetValidationErrorString(validationResult.Errors[0].ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var result = await this.userService.Register(userDto, accountDto);
                if (result.IsSuccessful)
                {
                    return this.Created(string.Empty, null);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(result.MessageCode));
                }
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody]LoginViewModel viewModel)
        {
            var model = new AccountDTO()
            {
                Email = viewModel.LoginEmail,
                Login = viewModel.LoginEmail,
                Password = viewModel.Password,
            };

            var validationResult = this.accountValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(
                    error.ErrorCode,
                    this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var result = this.userService.Authorize(model);
                if (result.IsSuccessful)
                {
                    await this.signInManager.SignInAsync(model.Login, result.Id);
                    return this.Ok();
                }

                this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(result.MessageCode));
            }

            return this.BadRequest(this.ModelState);
        }

        [Authorize]
        [HttpGet("Cabinet")]
        [ProducesResponseType(typeof(GetUserInfoResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Cabinet()
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = this.userService.GetUserById(userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            var viewModel = new UserInfoViewModel()
            {
                User = result.User,
                CompletedCourses = result.CompletedCourses,
                SkillLevels = result.SkillLevels.ToDictionary(
                    pair => pair.Key.Id,
                    pair => new UserInfoViewModel.SkillLevel() { Skill = pair.Key, Level = pair.Value }),

                JoinedCoursesProgress = result.JoinedCoursesProgress.ToDictionary(
                    pair => pair.Key.Id,
                    pair => new UserInfoViewModel.CourseProgress() { Course = pair.Key, ProgressPercent = pair.Value }),
            };

            return new ObjectResult(viewModel);
        }

        [Authorize]
        [HttpPost("Logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.Ok();
        }
    }
}
