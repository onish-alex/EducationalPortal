namespace EducationPortal.MVC.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Resources;
    using EducationPortal.MVC.Utilities;
    using FluentValidation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CourseController : Controller
    {
        private ICourseService courseService;
        private IUserService userService;
        private IResourceHelper resourceHelper;
        private IValidator<CourseDTO> courseValidator;
        private IValidator<SkillDTO> skillValidator;

        public CourseController(
            ICourseService courseService,
            IResourceHelper resourseHelper,
            IUserService userService,
            IValidator<CourseDTO> courseValidator,
            IValidator<SkillDTO> skillValidator)
        {
            this.courseService = courseService;
            this.userService = userService;
            this.resourceHelper = resourseHelper;
            this.courseValidator = courseValidator;
            this.skillValidator = skillValidator;
        }

        public IActionResult Index(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = this.courseService.GetCourseStatus(id, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Course", new { message = message });
            }

            var viewModel = new ConcreteCourseViewModel()
            {
                CourseInfo = result,
            };

            var detailInfoMessages = new List<string>();

            if (result.IsCompleted)
            {
                detailInfoMessages.Add(CommonMessages.OutputHasCompleteCourse);
            }

            if (result.IsJoined)
            {
                detailInfoMessages.Add(CommonMessages.OutputIsJoiningCourse);
            }

            if (result.IsCreator)
            {
                detailInfoMessages.Add(CommonMessages.OutputIsCourseAuthor);
            }

            viewModel.DetailInfo = detailInfoMessages;
            viewModel.Id = id;

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDTO model)
        {
            var validationResult = this.courseValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(error.ErrorCode));
                }
            }

            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.CreatorId = userId;

            if (this.ModelState.IsValid)
            {
                var result = await this.courseService.AddCourse(model);

                if (!result.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(result.MessageCode);
                    return this.RedirectToAction("Error", "Section", new { message = message });
                }

                return this.RedirectToAction("Index", new { id = model.Id });
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var result = this.courseService.GetCourse(id);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            return this.View(result.Course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseDTO model)
        {
            var validationResult = this.courseValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(error.ErrorCode));
                }
            }

            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.CreatorId = userId;

            if (this.ModelState.IsValid)
            {
                var result = await this.courseService.EditCourse(userId, model);

                if (!result.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(result.MessageCode);
                    return this.RedirectToAction("Error", "Section", new { message = message });
                }

                return this.RedirectToAction("Index", new { id = model.Id });
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult SkillEdit(long id)
        {
            var result = this.courseService.GetCourse(id);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (result.Course.CreatorId != userId)
            {
                var message = this.resourceHelper.GetMessageString("CanEditCourseNotAnAuthor");
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new SkillViewModel()
            {
                Course = result.Course,
                SkillsToRemove = new Dictionary<long, bool>(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkill([FromForm]SkillViewModel model)
        {
            var validationResult = this.skillValidator.Validate(
                model.Skill,
                cfg => cfg.IncludeRuleSets("Detail"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(error.ErrorCode));
                }
            }

            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (this.ModelState.IsValid)
            {
                var result = await this.courseService.AddSkill(userId, model.Course.Id, model.Skill);

                if (!result.IsSuccessful)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(result.MessageCode));
                    return this.View("SkillEdit", model);
                }

                return this.RedirectToAction("SkillEdit", new { id = model.Course.Id });
            }

            model.SkillsToRemove = new Dictionary<long, bool>();

            return this.View("SkillEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSkill(SkillViewModel model)
        {
            if (model.SkillsToRemove == null)
            {
                return this.RedirectToAction("SkillEdit", new { id = model.Course.Id });
            }

            var skillToRemoveNames = model.SkillsToRemove
                .Where(x => x.Value == true)
                .Select(x => x.Key);

            if (skillToRemoveNames.Count() == 0)
            {
                this.ModelState.AddModelError(string.Empty, CommonMessages.ErrorSkillToDeleteNotSelected);
                model.SkillsToRemove.Clear();
                return this.View("SkillEdit", model);
            }

            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            foreach (var skillId in skillToRemoveNames)
            {
                var result = await this.courseService.RemoveSkill(userId, model.Course.Id, skillId);

                if (!result.IsSuccessful)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetMessageString(result.MessageCode));
                    return this.View("SkillEdit", model);
                }
            }

            return this.RedirectToAction("SkillEdit", new { id = model.Course.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await this.courseService.DeleteCourse(userId, id);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            return this.RedirectToAction("Global", "Section");
        }

        [HttpGet]
        public async Task<IActionResult> Join(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await this.userService.JoinToCourse(userId, id);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            return this.RedirectToAction("Index", "Study", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmComplete(long id, string courseName)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var completionResult = await this.userService.AddCompletedCourse(userId, id);

            if (!completionResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(completionResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new CourseCompleteViewModel()
            {
                CourseName = courseName,
                RecievedSkills = completionResult.RecievedSkills,
                CourseId = id,
            };

            return this.View("Congratulations", viewModel);
        }
    }
}
