namespace EducationPortal.MVC.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Utilities;
    using EducationPortal.MVC.Models;
    using EducationPortal.MVC.Utilities;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class MaterialApiController : ControllerBase
    {
        private IMaterialService materialService;
        private ICourseService courseService;
        private IResourceHelper resourceHelper;
        private IValidator<MaterialDTO> materialValidator;
        private IDictionary<string, string> materialSubTypeViews;

        public MaterialApiController(
            IMaterialService materialService,
            ICourseService courseService,
            IResourceHelper resourceHelper,
            IValidator<MaterialDTO> materialValidator)
        {
            this.materialService = materialService;
            this.courseService = courseService;
            this.resourceHelper = resourceHelper;
            this.materialValidator = materialValidator;

            var materialTypeNames = Enum.GetNames(typeof(MaterialTypes));

            this.materialSubTypeViews = new Dictionary<string, string>();

            foreach (var name in materialTypeNames)
            {
                this.materialSubTypeViews.Add(name, string.Format("_{0}Editor", name));
            }
        }

        [HttpGet("FromCourse/{courseId}")]
        [ProducesResponseType(typeof(IEnumerable<MaterialDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> FromCourse(long courseId, int page = 1, int pageSize = 5)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var canEditResult = await this.courseService.CanEditCourse(userId, courseId);

            if (!canEditResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(canEditResult.MessageCode);
                this.ModelState.AddModelError(canEditResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            var courseMaterialsResult = await this.materialService.GetCourseMaterials(courseId, page, pageSize);

            if (!courseMaterialsResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(courseMaterialsResult.MessageCode);
                this.ModelState.AddModelError(courseMaterialsResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(courseMaterialsResult.Materials);
        }

        [HttpGet("Global/{courseId}")]
        [ProducesResponseType(typeof(IEnumerable<MaterialDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Global(long courseId, int page = 1, int pageSize = 5)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var canEditResult = await this.courseService.CanEditCourse(userId, courseId);

            if (!canEditResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(canEditResult.MessageCode);
                this.ModelState.AddModelError(canEditResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            var courseMaterialsResult = await this.materialService.GetGlobalMaterials(page, pageSize, courseId);

            if (!courseMaterialsResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(courseMaterialsResult.MessageCode);
                this.ModelState.AddModelError(courseMaterialsResult.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return new ObjectResult(courseMaterialsResult.Materials);
        }

        [HttpPost("AddExist")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddExistMaterial(AddRemoveMaterialViewModel model)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.AddMaterialToCourse(userId, model.CourseId, model.MaterialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }

        [HttpDelete("Remove")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> RemoveMaterial(AddRemoveMaterialViewModel model)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.RemoveMaterialFromCourse(userId, model.CourseId, model.MaterialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                this.ModelState.AddModelError(result.MessageCode, message);
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }

        [HttpPost("AddVideo/{courseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddVideo(long courseId, VideoDTO material)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var validationResult = this.materialValidator.Validate(material, opt => opt.IncludeRuleSets("Common", "Video"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(error.ErrorCode, this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var addMaterialResult = await this.materialService.AddMaterial(material);

                if (!addMaterialResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addMaterialResult.MessageCode);
                    this.ModelState.AddModelError(addMaterialResult.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                var addToCourseResult = await this.courseService.AddMaterialToCourse(userId, courseId, addMaterialResult.MaterialId);
                if (!addToCourseResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addToCourseResult.MessageCode);
                    this.ModelState.AddModelError(addToCourseResult.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                return this.Ok();
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPost("AddArticle/{courseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddArticle(long courseId, ArticleDTO material)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var validationResult = this.materialValidator.Validate(material, opt => opt.IncludeRuleSets("Common", "Article"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(error.ErrorCode, this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var addMaterialResult = await this.materialService.AddMaterial(material);

                if (!addMaterialResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addMaterialResult.MessageCode);
                    this.ModelState.AddModelError(addMaterialResult.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                var addToCourseResult = await this.courseService.AddMaterialToCourse(userId, courseId, addMaterialResult.MaterialId);
                if (!addToCourseResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addToCourseResult.MessageCode);
                    this.ModelState.AddModelError(addToCourseResult.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                return this.Ok();
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPost("AddBook/{courseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddBook(long courseId, BookDTO material)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var validationResult = this.materialValidator.Validate(material, opt => opt.IncludeRuleSets("Common", "Book"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(error.ErrorCode, this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var addMaterialResult = await this.materialService.AddMaterial(material);

                if (!addMaterialResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addMaterialResult.MessageCode);
                    this.ModelState.AddModelError(addMaterialResult.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                var addToCourseResult = await this.courseService.AddMaterialToCourse(userId, courseId, addMaterialResult.MaterialId);
                if (!addToCourseResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addToCourseResult.MessageCode);
                    this.ModelState.AddModelError(addToCourseResult.MessageCode, message);
                    return this.BadRequest(this.ModelState);
                }

                return this.Ok();
            }

            return this.BadRequest(this.ModelState);
        }
    }
}
