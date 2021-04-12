namespace EducationPortal.MVC.Controllers.Api
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Utilities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudyApiController : ControllerBase
    {
        private ICourseService courseService;
        private IUserService userService;
        private IMaterialService materialService;
        private IResourceHelper resourceHelper;

        public StudyApiController(
            ICourseService courseService,
            IUserService userService,
            IMaterialService materialService,
            IResourceHelper resourceHelper)
        {
            this.courseService = courseService;
            this.userService = userService;
            this.materialService = materialService;
            this.resourceHelper = resourceHelper;
        }

        [HttpGet("{courseId}")]
        [ProducesResponseType(typeof(CourseStudyApiViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult CourseMaterials(long courseId, int page = 1, int pageSize = 10)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var materialPageResult = this.materialService.GetMaterialsToStudy(courseId, userId, page, pageSize);

            if (!materialPageResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(materialPageResult.MessageCode);
                this.ModelState.AddModelError(materialPageResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            var courseCompletenessResult = this.courseService.CheckCourseCompletness(courseId, userId);

            if (!courseCompletenessResult.IsSuccessful
             && courseCompletenessResult.MessageCode != null)
            {
                var message = this.resourceHelper.GetMessageString(courseCompletenessResult.MessageCode);
                this.ModelState.AddModelError(courseCompletenessResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            var viewModel = new CourseStudyApiViewModel()
            {
                Materials = materialPageResult.Materials,
                MaterialCompleteness = materialPageResult.MaterialStatuses.ToDictionary(key => key.Key.Id, value => value.Value),
                CourseId = courseId,
                IsCompleted = courseCompletenessResult.IsSuccessful,
            };

            return new ObjectResult(viewModel);
        }

        [HttpPost("Learn")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Learn(LearnMaterialViewModel model)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.userService.LearnMaterial(userId, model.CourseId, model.MaterialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }
    }
}
