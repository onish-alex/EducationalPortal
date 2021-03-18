namespace EducationPortal.MVC.Controllers.Api
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Utilities;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class SectionApiController : ControllerBase
    {
        private ICourseService courseService;
        private IResourceHelper resourceHelper;

        public SectionApiController(
            ICourseService courseService,
            IResourceHelper resourceHelper)
        {
            this.courseService = courseService;
            this.resourceHelper = resourceHelper;
        }

        [HttpGet("Global")]
        [ProducesResponseType(typeof(IEnumerable<CourseDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Global(int page = 1, int pageSize = 12)
        {
            var result = await this.courseService.GetGlobalCourses(page, pageSize);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(result.Courses);
        }

        [HttpGet("Created")]
        [ProducesResponseType(typeof(IEnumerable<CourseDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Created(int page = 1, int pageSize = 12)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.GetUserCourses(page, pageSize, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(result.Courses);
        }

        [HttpGet("Completed")]
        [ProducesResponseType(typeof(IEnumerable<CourseDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Completed(int page = 1, int pageSize = 12)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.GetCompletedCourses(page, pageSize, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(result.Courses);
        }

        [HttpGet("Joined")]
        [ProducesResponseType(typeof(IEnumerable<CourseDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Joined(int page = 1, int pageSize = 12)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.GetJoinedCourses(page, pageSize, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(result.Courses);
        }
    }
}
