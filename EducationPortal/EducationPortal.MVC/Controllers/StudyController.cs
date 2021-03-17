namespace EducationPortal.MVC.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Utilities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class StudyController : Controller
    {
        private ICourseService courseService;
        private IUserService userService;
        private IMaterialService materialService;
        private IResourceHelper resourceHelper;

        public StudyController(
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

        public IActionResult Index(long id, int page = 1, int pageSize = 10)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var materialPageResult = this.materialService.GetMaterialsToStudy(id, userId, page, pageSize);

            if (!materialPageResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(materialPageResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var courseCompletenessResult = this.courseService.CheckCourseCompletness(id, userId);

            if (!courseCompletenessResult.IsSuccessful
             && courseCompletenessResult.MessageCode != null)
            {
                var message = this.resourceHelper.GetMessageString(courseCompletenessResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new CourseStudyViewModel()
            {
                PageData = materialPageResult,
                CourseId = id,
                IsCompleted = courseCompletenessResult.IsSuccessful,
            };

            return this.View(viewModel);
        }

        public IActionResult Material(long courseId, long materialId, int page = 1, int pageSize = 10)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = this.materialService.GetMaterialsToStudy(courseId, userId, page, pageSize);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new CourseStudyViewModel()
            {
                PageData = result,
                CourseId = courseId,
                ChosenMaterial = result.Materials.SingleOrDefault(x => x.Id == materialId),
            };

            return this.View("Index", viewModel);
        }

        public async Task<IActionResult> Learn(long materialId, long courseId)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.userService.LearnMaterial(userId, courseId, materialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            return this.RedirectToAction("Index", new { id = courseId });
        }
    }
}
