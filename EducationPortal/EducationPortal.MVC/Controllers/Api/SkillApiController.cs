namespace EducationPortal.MVC.Controllers.Api
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

    [Route("api/[controller]")]
    [ApiController]
    public class SkillApiController : ControllerBase
    {
        private ICourseService courseService;
        private IResourceHelper resourceHelper;
        private IValidator<SkillDTO> skillValidator;

        public SkillApiController(
            ICourseService courseService,
            IResourceHelper resourseHelper,
            IValidator<SkillDTO> skillValidator)
        {
            this.courseService = courseService;
            this.resourceHelper = resourseHelper;
            this.skillValidator = skillValidator;
        }

        [HttpGet("{courseId:long}")]
        [ProducesResponseType(typeof(IEnumerable<SkillDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(long courseId)
        {
            var result = this.courseService.GetCourse(courseId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(result.Course.Skills);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Post(SkillCreateViewModel model)
        {
            var skill = new SkillDTO()
            {
                Name = model.SkillName,
            };

            var validationResult = this.skillValidator.Validate(
                skill,
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
                var result = await this.courseService.AddSkill(userId, model.CourseId, skill);

                if (!result.IsSuccessful)
                {
                    this.ModelState.AddModelError(result.MessageCode, this.resourceHelper.GetMessageString(result.MessageCode));
                    return this.BadRequest(this.ModelState);
                }

                return this.Ok();
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(SkillDeleteViewModel model)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.RemoveSkill(userId, model.CourseId, model.SkillId);

            if (!result.IsSuccessful)
            {
                this.ModelState.AddModelError(result.MessageCode, this.resourceHelper.GetMessageString(result.MessageCode));
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }
    }
}
