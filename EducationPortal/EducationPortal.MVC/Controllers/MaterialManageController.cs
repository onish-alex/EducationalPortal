namespace EducationPortal.MVC.Controllers
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
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MaterialManageController : Controller
    {
        private IMaterialService materialService;
        private ICourseService courseService;
        private IResourceHelper resourceHelper;
        private IValidator<MaterialDTO> materialValidator;
        private IDictionary<string, string> materialSubTypeViews;

        public MaterialManageController(
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

        public async Task<IActionResult> Index(long id, int page = 1, int pageSize = 5)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var canEditResult = await this.courseService.CanEditCourse(userId, id);

            if (!canEditResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(canEditResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var globalMaterialsResult = await this.materialService.GetGlobalMaterials(page, pageSize, id);

            if (!globalMaterialsResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(globalMaterialsResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var courseMaterialsResult = await this.materialService.GetCourseMaterials(id, page, pageSize);

            if (!courseMaterialsResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(courseMaterialsResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var materialTypeNames = Enum.GetNames(typeof(MaterialTypes));
            var materialTypeSelectList = new List<SelectListItem>();

            foreach (var item in materialTypeNames)
            {
                materialTypeSelectList.Add(new SelectListItem()
                {
                    Text = this.resourceHelper.GetCommonContentString(item),
                    Value = item,
                });
            }

            var viewModel = new MaterialManageViewModel()
            {
                CourseMaterials = courseMaterialsResult.Materials,
                GlobalMaterials = globalMaterialsResult.Materials,
                CourseId = id,
                MaterialTypeNames = materialTypeSelectList,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CourseMaterialPage([FromForm]int id, [FromForm]int page, [FromForm]int pageSize)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var courseMaterialsResult = await this.materialService.GetCourseMaterials(id, page, pageSize);

            if (!courseMaterialsResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(courseMaterialsResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new MaterialListViewModel()
            {
                ActionPath = "/MaterialManage/CourseMaterialPage",
                DomUpdateId = "#course-materials",
                DomSelectClass = "course-material-item",
                CourseId = id,
                Materials = courseMaterialsResult.Materials,
            };

            return this.PartialView("_MaterialList", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GlobalMaterialPage([FromForm] int id, [FromForm] int page, [FromForm] int pageSize)
        {
            var globalMaterialsResult = await this.materialService.GetGlobalMaterials(page, pageSize, id);

            if (!globalMaterialsResult.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(globalMaterialsResult.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new MaterialListViewModel()
            {
                ActionPath = "/MaterialManage/GlobalMaterialPage",
                DomUpdateId = "#global-materials",
                DomSelectClass = "global-material-item",
                CourseId = id,
                Materials = globalMaterialsResult.Materials,
            };

            return this.PartialView("_MaterialList", viewModel);
        }

        [HttpPost]
        public IActionResult Editor(string materialType)
        {
            return this.PartialView(this.materialSubTypeViews[materialType]);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseFromGlobal(long courseId, long materialId)
        {
            var result = await this.materialService.GetMaterial(materialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new AddMaterialViewModel()
            {
                CourseId = courseId,
                Material = result.Material,
            };

            return this.PartialView("_MaterialToAdd", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseFromCourse(long courseId, long materialId)
        {
            var result = await this.materialService.GetMaterial(materialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            var viewModel = new AddMaterialViewModel()
            {
                CourseId = courseId,
                Material = result.Material,
            };

            return this.PartialView("_MaterialToRemove", viewModel);
        }

        public async Task<IActionResult> AddMaterial(long courseId, long materialId)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.AddMaterialToCourse(userId, courseId, materialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            return this.RedirectToAction("Index", new { id = courseId });
        }

        public async Task<IActionResult> RemoveMaterial(long courseId, long materialId)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await this.courseService.RemoveMaterialFromCourse(userId, courseId, materialId);

            if (!result.IsSuccessful)
            {
                var message = this.resourceHelper.GetMessageString(result.MessageCode);
                return this.RedirectToAction("Error", "Section", new { message = message });
            }

            return this.RedirectToAction("Index", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideo(long courseId, VideoDTO material)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var validationResult = this.materialValidator.Validate(material, opt => opt.IncludeRuleSets("Common", "Video"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var addMaterialResult = await this.materialService.AddMaterial(material);

                if (!addMaterialResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addMaterialResult.MessageCode);
                    this.ModelState.AddModelError(string.Empty, message);
                }

                var addToCourseResult = await this.courseService.AddMaterialToCourse(userId, courseId, addMaterialResult.MaterialId);
                if (!addToCourseResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addToCourseResult.MessageCode);
                    this.ModelState.AddModelError(string.Empty, message);
                }
            }

            return this.PartialView("_VideoEditor", material);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(long courseId, ArticleDTO material)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var validationResult = this.materialValidator.Validate(material, opt => opt.IncludeRuleSets("Common", "Article"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var addMaterialResult = await this.materialService.AddMaterial(material);

                if (!addMaterialResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addMaterialResult.MessageCode);
                    this.ModelState.AddModelError(string.Empty, message);
                }

                var addToCourseResult = await this.courseService.AddMaterialToCourse(userId, courseId, addMaterialResult.MaterialId);
                if (!addToCourseResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addToCourseResult.MessageCode);
                    this.ModelState.AddModelError(string.Empty, message);
                }
            }

            return this.PartialView("_ArticleEditor", material);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(long courseId, BookDTO material)
        {
            var userId = long.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var validationResult = this.materialValidator.Validate(material, opt => opt.IncludeRuleSets("Common", "Book"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, this.resourceHelper.GetValidationErrorString(error.ErrorCode));
                }
            }

            if (this.ModelState.IsValid)
            {
                var addMaterialResult = await this.materialService.AddMaterial(material);

                if (!addMaterialResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addMaterialResult.MessageCode);
                    this.ModelState.AddModelError(string.Empty, message);
                }

                var addToCourseResult = await this.courseService.AddMaterialToCourse(userId, courseId, addMaterialResult.MaterialId);
                if (!addToCourseResult.IsSuccessful)
                {
                    var message = this.resourceHelper.GetMessageString(addToCourseResult.MessageCode);
                    this.ModelState.AddModelError(string.Empty, message);
                }
            }

            return this.PartialView("_BookEditor", material);
        }
    }
}