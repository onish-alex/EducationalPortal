namespace EducationPortal.MVC.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Utilities;
    using FluentValidation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class SectionController : Controller
    {
        private ICourseService courseService;
        private IResourceHelper resourseHelper;

        public SectionController(
            ICourseService courseService,
            IResourceHelper resourseHelper)
        {
            this.courseService = courseService;
            this.resourseHelper = resourseHelper;
        }

        public async Task<IActionResult> Global(int page = 1, int pageSize = 12)
        {
            var result = await this.courseService.GetGlobalCourses(page, pageSize);

            if (!result.IsSuccessful)
            {
                var message = this.resourseHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", new { message = message });
            }

            return this.View(result.Courses);
        }

        public async Task<IActionResult> Created(int page = 1, int pageSize = 12)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.GetUserCourses(page, pageSize, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourseHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", new { message = message });
            }

            return this.View(result.Courses);
        }

        public async Task<IActionResult> Completed(int page = 1, int pageSize = 12)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.GetCompletedCourses(page, pageSize, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourseHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", new { message = message });
            }

            return this.View(result.Courses);
        }

        public async Task<IActionResult> Joined(int page = 1, int pageSize = 12)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.GetJoinedCourses(page, pageSize, userId);

            if (!result.IsSuccessful)
            {
                var message = this.resourseHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", new { message = message });
            }

            return this.View(result.Courses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return this.View(new ErrorViewModel
            {
                Message = message ?? string.Empty,
            });
        }
    }
}
