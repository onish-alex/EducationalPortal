namespace EducationPortal.BLL.Services
{
    using System;
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

    public class CourseService : ICourseService
    {
        private IRepository<Course> courseRepository;
        private IRepository<Skill> skillRepository;
        private IRepository<Material> materialRepository;
        private IRepository<User> userRepository;
        private IMapper mapper;
        private IValidator<CourseDTO> courseValidator;
        private IValidator<SkillDTO> skillValidator;

        public CourseService(
            IRepository<Course> courseRepository,
            IRepository<Skill> skillRepository,
            IRepository<Material> materialRepository,
            IRepository<User> userRepository,
            IMapper mapper,
            IValidator<CourseDTO> courseValidator,
            IValidator<SkillDTO> skillValidator)
        {
            this.courseRepository = courseRepository;
            this.skillRepository = skillRepository;
            this.materialRepository = materialRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.courseValidator = courseValidator;
            this.skillValidator = skillValidator;
        }

        public async Task<OperationResult> AddCourse(CourseDTO course)
        {
            var result = new OperationResult("AddCourseSuccess", true);

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNull";
                return result;
            }

            var validationResult = this.courseValidator.Validate(course);

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            if (this.courseRepository.Find(x => x.Name == course.Name).SingleOrDefault() != null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseAlreadyExist";
                return result;
            }

            var courseToAdd = this.mapper.Map<CourseDTO, Course>(course);

            await this.courseRepository.CreateAsync(courseToAdd);
            await this.courseRepository.SaveAsync();

            course.Id = courseToAdd.Id;

            return result;
        }

        public GetCoursesResult GetUserCourses(long userId)
        {
            var result = new GetCoursesResult();

            var userCourses = this.courseRepository.Find(
                course => course.CreatorId == userId,
                course => course.Skills);

            result.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);
            return result;
        }

        public GetCoursesResult GetAllCourses()
        {
            var result = new GetCoursesResult();

            var userCourses = this.courseRepository.GetAll(course => course.Skills);
            result.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);

            return result;
        }

        public async Task<OperationResult> EditCourse(long userId, CourseDTO newCourseInfo)
        {
            if (newCourseInfo == null)
            {
                return new OperationResult("CourseNull", false);
            }

            var result = await this.CanEditCourse(userId, newCourseInfo.Id);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var validationResult = this.courseValidator.Validate(newCourseInfo);

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var courseToUpdate = await this.courseRepository.GetByIdAsync(newCourseInfo.Id);
            courseToUpdate.Name = newCourseInfo.Name;
            courseToUpdate.Description = newCourseInfo.Description;

            this.courseRepository.Update(courseToUpdate);
            await this.courseRepository.SaveAsync();

            result.MessageCode = "EditCourseSuccess";
            result.IsSuccessful = true;

            return result;
        }

        public async Task<OperationResult> AddSkill(long userId, long courseId, SkillDTO skill)
        {
            if (skill == null)
            {
                return new OperationResult("SkillNull", false);
            }

            var result = await this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var validationResult = this.skillValidator.Validate(skill, "Base", "Detail");

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Skills)
                .SingleOrDefault();

            var skillToAdd = this.skillRepository.Find(sk => sk.Name.ToLower() == skill.Name.ToLower()).SingleOrDefault();

            if (skillToAdd == null)
            {
                skillToAdd = this.mapper.Map<SkillDTO, Skill>(skill);
                await this.skillRepository.CreateAsync(skillToAdd);
                await this.skillRepository.SaveAsync();
                skill.Id = skillToAdd.Id;
            }
            else if (course.Skills.Contains(skillToAdd))
            {
                result.IsSuccessful = false;
                result.MessageCode = "AddSkillAlreadyExists";
                return result;
            }

            course.Skills.Add(skillToAdd);

            this.courseRepository.Update(course);
            await this.courseRepository.SaveAsync();

            result.IsSuccessful = true;
            result.MessageCode = "AddSkillSuccess";

            return result;
        }

        public async Task<OperationResult> RemoveSkill(long userId, long courseId, long skillId)
        {
            var result = await this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Skills)
                .SingleOrDefault();

            var skillToRemove = await this.skillRepository.GetByIdAsync(skillId);

            if (skillToRemove == null)
            {
                result.MessageCode = "RemoveSkillNotFound";
                result.IsSuccessful = false;
                return result;
            }

            course.Skills.Remove(skillToRemove);
            this.courseRepository.Update(course);
            await this.courseRepository.SaveAsync();

            result.IsSuccessful = true;
            result.MessageCode = "RemoveSkillSuccess";

            return result;
        }

        public async Task<OperationResult> AddMaterialToCourse(long userId, long courseId, long materialId)
        {
            var result = await this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var course = this.courseRepository.Find(
                            course => course.Id == courseId,
                            course => course.Materials)
                            .SingleOrDefault();

            if (course.Materials.Any(x => x.Id == materialId))
            {
                result.MessageCode = "AddMaterialToCourseAlreadyExists";
                result.IsSuccessful = false;
                return result;
            }

            var material = await this.materialRepository.GetByIdAsync(materialId);

            if (material == null)
            {
                result.MessageCode = "MaterialNotFound";
                result.IsSuccessful = false;
                return result;
            }

            course.Materials.Add(material);

            this.courseRepository.Update(course);
            await this.courseRepository.SaveAsync();

            result.IsSuccessful = true;
            result.MessageCode = "AddMaterialToCourseSuccess";

            return result;
        }

        public async Task<OperationResult> CanEditCourse(long userId, long courseId)
        {
            var result = new OperationResult();

            var course = await this.courseRepository.GetByIdAsync(courseId);

            if (course == null)
            {
                result.MessageCode = "CourseNotFound";
                result.IsSuccessful = false;
                return result;
            }

            if (course.CreatorId != userId)
            {
                result.MessageCode = "CanEditCourseNotAnAuthor";
                result.IsSuccessful = false;
                return result;
            }

            result.IsSuccessful = true;
            return result;
        }

        public GetCourseStatusResult GetCourseStatus(long courseId, long userId)
        {
            var result = new GetCourseStatusResult();

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.CompletedUsers,
                course => course.JoinedUsers,
                course => course.Creator,
                course => course.Skills,
                course => course.Materials)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            if (course.CreatorId == userId)
            {
                result.IsCreator = true;
            }

            if (course.CompletedUsers.Any(x => x.UserId == userId))
            {
                result.IsCompleted = true;
            }

            if (course.JoinedUsers.Any(x => x.UserId == userId))
            {
                result.IsJoined = true;
            }

            var completnessResult = this.CheckCourseCompletness(courseId, userId);

            if (completnessResult.IsSuccessful)
            {
                result.IsReadyToComplete = true;
            }

            result.Name = course.Name;
            result.Description = course.Description;
            result.CreatorName = course.Creator.Name;
            result.SkillNames = this.mapper.Map<Skill, SkillDTO>(course.Skills).Select(x => x.Name);
            result.HasMaterials = course.Materials.Count != 0;
            result.IsSuccessful = true;

            return result;
        }

        public async Task<GetCoursesResult> GetGlobalCourses(int page, int pageSize)
        {
            var result = new GetCoursesResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            var maxPages = (int)Math.Ceiling(await this.courseRepository.CountAsync() / (double)pageSize);

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

            var userCourses = this.courseRepository.GetPage(page, pageSize, course => course.Skills);
            var courseDtos = this.mapper.Map<Course, CourseDTO>(userCourses);

            result.Courses = new PaginatedList<CourseDTO>(courseDtos, page, pageSize, maxPages);
            result.IsSuccessful = true;

            return result;
        }

        public GetSingleCourseResult GetCourse(long id)
        {
            var result = new GetSingleCourseResult();

            var course = this.courseRepository.Find(
                x => x.Id == id,
                x => x.Skills)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            result.Course = this.mapper.Map<Course, CourseDTO>(course);
            result.IsSuccessful = true;

            return result;
        }

        public async Task<OperationResult> DeleteCourse(long userId, long courseId)
        {
            var result = await this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var courseToDelete = await this.courseRepository.GetByIdAsync(courseId);

            if (courseToDelete == null)
            {
                result.MessageCode = "CourseNotFound";
                result.IsSuccessful = false;
                return result;
            }

            await this.courseRepository.DeleteAsync(courseId);
            await this.courseRepository.SaveAsync();
            result.IsSuccessful = true;

            return result;
        }

        public async Task<GetCoursesResult> GetUserCourses(int page, int pageSize, long userId)
        {
            var result = new GetCoursesResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            Expression<Func<Course, bool>> creatorPredicate = course => course.CreatorId == userId;

            var maxPages = (int)Math.Ceiling(await this.courseRepository.CountAsync(creatorPredicate) / (double)pageSize);

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

            var userCourses = this.courseRepository.GetPage(
                page,
                pageSize,
                creatorPredicate,
                course => course.Skills);

            var courseDtos = this.mapper.Map<Course, CourseDTO>(userCourses);

            result.Courses = new PaginatedList<CourseDTO>(courseDtos, page, pageSize, maxPages);
            result.IsSuccessful = true;

            return result;
        }

        public async Task<GetCoursesResult> GetJoinedCourses(int page, int pageSize, long userId)
        {
            var result = new GetCoursesResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            Expression<Func<Course, bool>> joinedPredicate = course => course.JoinedUsers.Any(x => x.UserId == userId);

            var maxPages = (int)Math.Ceiling(await this.courseRepository.CountAsync(joinedPredicate) / (double)pageSize);

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

            var joinedCourses = this.courseRepository.GetPage(
                page,
                pageSize,
                joinedPredicate,
                course => course.Skills,
                course => course.JoinedUsers);

            var courseDtos = this.mapper.Map<Course, CourseDTO>(joinedCourses);

            result.Courses = new PaginatedList<CourseDTO>(courseDtos, page, pageSize, maxPages);
            result.IsSuccessful = true;

            return result;
        }

        public async Task<GetCoursesResult> GetCompletedCourses(int page, int pageSize, long userId)
        {
            var result = new GetCoursesResult();

            if (pageSize <= 0)
            {
                result.IsSuccessful = false;
                result.MessageCode = "WrongPage";
                return result;
            }

            Expression<Func<Course, bool>> completedPredicate = course => course.CompletedUsers.Any(x => x.UserId == userId);

            var maxPages = (int)Math.Ceiling(await this.courseRepository.CountAsync(completedPredicate) / (double)pageSize);

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

            var completedCourses = this.courseRepository.GetPage(
                page,
                pageSize,
                completedPredicate,
                course => course.Skills,
                course => course.CompletedUsers);

            var courseDtos = this.mapper.Map<Course, CourseDTO>(completedCourses);

            result.Courses = new PaginatedList<CourseDTO>(courseDtos, page, pageSize, maxPages);
            result.IsSuccessful = true;

            return result;
        }

        public OperationResult CheckCourseCompletness(long courseId, long userId)
        {
            var result = new OperationResult();

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Materials)
                .SingleOrDefault();

            if (course == null)
            {
                result.MessageCode = "CourseNotFound";
                result.IsSuccessful = false;
                return result;
            }

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.LearnedMaterials)
                .SingleOrDefault();

            if (user == null)
            {
                result.MessageCode = "UserNotFound";
                result.IsSuccessful = false;
                return result;
            }

            var currentCourseLearnedMaterials = user.LearnedMaterials.Intersect(course.Materials);

            if (course.Materials.All(x => currentCourseLearnedMaterials.Contains(x)))
            {
                result.IsSuccessful = true;
            }

            return result;
        }

        public async Task<OperationResult> RemoveMaterialFromCourse(long userId, long courseId, long materialId)
        {
            var result = await this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var course = this.courseRepository.Find(
                            course => course.Id == courseId,
                            course => course.Materials)
                            .SingleOrDefault();

            var material = await this.materialRepository.GetByIdAsync(materialId);

            if (material == null)
            {
                result.MessageCode = "MaterialNotFound";
                result.IsSuccessful = false;
                return result;
            }

            if (!course.Materials.Contains(material))
            {
                result.MessageCode = "RemoveMaterialFromCourseNotContain";
                result.IsSuccessful = false;
                return result;
            }

            course.Materials.Remove(material);
            this.courseRepository.Update(course);
            await this.courseRepository.SaveAsync();

            result.IsSuccessful = true;
            return result;
        }
    }
}
