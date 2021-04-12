namespace EducationPortal.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Results;
    using EducationPortal.BLL.Utilities;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;
    using FluentValidation;

    public class MaterialService : IMaterialService
    {
        private IRepository<Material> materialRepository;
        private IRepository<User> userRepository;
        private IRepository<Course> courseRepository;
        private IMapper mapper;
        private IValidator<MaterialDTO> materialValidator;

        public MaterialService(
            IRepository<Material> materialRepository,
            IRepository<User> userRepository,
            IRepository<Course> courseRepository,
            IMapper mapper,
            IValidator<MaterialDTO> materialValidator)
        {
            this.materialRepository = materialRepository;
            this.userRepository = userRepository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
            this.materialValidator = materialValidator;
        }

        public async Task<AddMaterialResult> AddMaterial(MaterialDTO material)
        {
            var result = new AddMaterialResult();

            if (material == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "MaterialNull";
                return result;
            }

            var validationRuleSetStr = string.Empty;

            switch (material)
            {
                case ArticleDTO _:
                    validationRuleSetStr = "Article";
                    break;

                case BookDTO _:
                    validationRuleSetStr = "Book";
                    break;

                case VideoDTO _:
                    validationRuleSetStr = "Video";
                    break;
            }

            var validationResult = this.materialValidator.Validate(material, "Common", validationRuleSetStr);

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var materialToAdd = this.mapper.Map<MaterialDTO, Material>(material);

            this.materialRepository.Create(materialToAdd);
            await this.materialRepository.SaveAsync();

            result.MaterialId = materialToAdd.Id;
            result.IsSuccessful = true;

            return result;
        }

        public GetMaterialsResult GetAllMaterials()
        {
            var result = new GetMaterialsResult();

            var allMaterials = this.materialRepository.GetAll();

            var allMaterialDTOs = new List<MaterialDTO>();

            foreach (var item in allMaterials)
            {
                var materialToAdd = this.mapper.Map<Material, MaterialDTO>(item);
                allMaterialDTOs.Add(materialToAdd);
            }

            result.IsSuccessful = allMaterialDTOs.Count != 0;

            if (!result.IsSuccessful)
            {
                result.MessageCode = "GetAllMaterialsEmptyResult";
            }

            result.Materials = allMaterialDTOs;

            return result;
        }

        public async Task<OperationResult> CheckMaterialExisting(long materialId)
        {
            var result = new OperationResult();
            var materialToCheck = await this.materialRepository.GetByIdAsync(materialId);
            if (materialToCheck == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CheckMaterialExistingNotFound";
            }

            result.IsSuccessful = true;

            return result;
        }

        public GetMaterialPageResult GetMaterialsToStudy(long courseId, long userId, int page, int pageSize)
        {
            var result = new GetMaterialPageResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.JoinedUsers,
                course => course.CompletedUsers,
                course => course.Materials)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            var maxPages = (int)Math.Ceiling(course.Materials.Count / (double)pageSize);

            if (page < 1 || page > maxPages)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.LearnedMaterials)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            IEnumerable<MaterialDTO> materialDtos;
            var learnedMaterialIds = user.LearnedMaterials.Select(x => x.Id);

            if (course.JoinedUsers.Any(x => x.UserId == userId))
            {
                var materialPage = this.materialRepository.GetPage(
                    page,
                    pageSize,
                    material => course.Materials.Contains(material));

                materialDtos = this.mapper.Map<Material, MaterialDTO>(materialPage);

                result.MaterialStatuses = materialDtos.ToDictionary(
                    key => key,
                    value => learnedMaterialIds.Contains(value.Id));
            }
            else if (course.CompletedUsers.Any(x => x.UserId == userId))
            {
                var learnedMaterials = user.LearnedMaterials.Intersect(course.Materials);

                var materialPage = this.materialRepository.GetPage(
                    page,
                    pageSize,
                    material => learnedMaterialIds.Contains(material.Id));

                materialDtos = this.mapper.Map<Material, MaterialDTO>(materialPage);

                result.MaterialStatuses = materialDtos.ToDictionary(
                    key => key,
                    value => true);
            }
            else
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotJoined";
                return result;
            }

            result.Materials = new PaginatedList<MaterialDTO>(
                materialDtos,
                page,
                pageSize,
                maxPages);

            result.IsSuccessful = true;

            return result;
        }

        public async Task<GetMaterialPageResult> GetGlobalMaterials(int page, int pageSize, long? courseId = null)
        {
            var result = new GetMaterialPageResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            Course course = null;

            if (courseId != null)
            {
                course = this.courseRepository.Find(
                    course => course.Id == courseId.Value,
                    course => course.Materials)
                    .SingleOrDefault();
            }

            if (course == null && courseId != null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            int maxPages;

            if (courseId == null)
            {
                maxPages = (int)Math.Ceiling(await this.materialRepository.CountAsync() / (double)pageSize);
            }
            else
            {
                maxPages = (int)Math.Ceiling((await this.materialRepository.CountAsync() - course.Materials.Count) / (double)pageSize);
            }

            if (maxPages == 0)
            {
                maxPages++;
            }

            if (page < 1 || page > maxPages)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            IEnumerable<Material> materialPage;

            if (courseId == null)
            {
                materialPage = this.materialRepository.GetPage(page, pageSize);
            }
            else
            {
                materialPage = this.materialRepository.GetPage(
                page,
                pageSize,
                material => !course.Materials.Contains(material));
            }

            var materialDtos = this.mapper.Map<Material, MaterialDTO>(materialPage);

            result.Materials = new PaginatedList<MaterialDTO>(
                materialDtos,
                page,
                pageSize,
                maxPages);

            result.IsSuccessful = true;

            return result;
        }

        public async Task<GetMaterialPageResult> GetCourseMaterials(long courseId, int page, int pageSize)
        {
            var result = new GetMaterialPageResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Materials)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            var maxPages = (int)Math.Ceiling(await this.materialRepository.CountAsync(material => course.Materials.Contains(material)) / (double)pageSize);

            if (maxPages == 0)
            {
                maxPages++;
            }

            if (page < 1 || page > maxPages)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            var materialPage = this.materialRepository.GetPage(
                page,
                pageSize,
                material => course.Materials.Contains(material));

            var materialDtos = this.mapper.Map<Material, MaterialDTO>(materialPage);

            result.Materials = new PaginatedList<MaterialDTO>(
                materialDtos,
                page,
                pageSize,
                maxPages);

            result.IsSuccessful = true;
            return result;
        }

        public async Task<GetSingleMaterialResult> GetMaterial(long id)
        {
            var result = new GetSingleMaterialResult();

            var material = await this.materialRepository.GetByIdAsync(id);

            if (material == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "MaterialNotFound";
            }

            result.Material = this.mapper.Map<Material, MaterialDTO>(material);
            result.IsSuccessful = true;

            return result;
        }
    }
}
