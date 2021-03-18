namespace EducationPortal.MVC.Controllers.Api
{
    using System.Collections.Generic;
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

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseApiController : ControllerBase
    {
        private ICourseService courseService;
        private IUserService userService;
        private IResourceHelper resourceHelper;
        private IValidator<CourseDTO> courseValidator;

        public CourseApiController(
            ICourseService courseService,
            IResourceHelper resourseHelper,
            IUserService userService,
            IValidator<CourseDTO> courseValidator)
        {
            this.courseService = courseService;
            this.userService = userService;
            this.resourceHelper = resourseHelper;
            this.courseValidator = courseValidator;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ConcreteCourseViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = this.courseService.GetCourseStatus(id, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
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

            return new ObjectResult(viewModel);
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(ConcreteCourseViewModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create(CourseCreateUpdateViewModel viewModel)
        {
            var model = new CourseDTO()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
            };

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
                    this.ModelState.AddModelError(result.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                return this.Created($"api/Course/{model.Id}", model);
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Edit(long id, CourseCreateUpdateViewModel viewModel)
        {
            var model = new CourseDTO()
            {
                Id = id,
                Name = viewModel.Name,
                Description = viewModel.Description,
            };

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
                    this.ModelState.AddModelError(result.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                return this.Ok();
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await this.courseService.DeleteCourse(userId, id);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }

        [HttpGet("Join")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Join(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await this.userService.JoinToCourse(userId, id);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }

        [HttpGet("ConfirmComplete")]
        [ProducesResponseType(typeof(CourseCompleteViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ConfirmComplete(long id)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var completionResult = await this.userService.AddCompletedCourse(userId, id);

            if (!completionResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(completionResult.MessageCode);
                this.ModelState.AddModelError(completionResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            var courseResult = this.courseService.GetCourse(id);

            var viewModel = new CourseCompleteViewModel()
            {
                CourseName = courseResult.Course.Name,
                RecievedSkills = completionResult.RecievedSkills,
                CourseId = courseResult.Course.Id,
            };

            return new ObjectResult(viewModel);
        }
    }
}
