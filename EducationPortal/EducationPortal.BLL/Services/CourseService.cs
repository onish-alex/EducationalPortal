namespace EducationPortal.BLL.Services
{
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Validation;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;

    public class CourseService : ICourseService
    {
        private IRepository<Course> courses;
        private IRepository<Skill> skills;
        private IRepository<Material> materials;
        private IMapper mapper;
        private IValidator<CourseDTO> courseValidator;
        private IValidator<SkillDTO> skillValidator;

        public CourseService(
            IRepository<Course> courses,
            IRepository<Skill> skills,
            IRepository<Material> materials,
            IMapper mapper,
            IValidator<CourseDTO> courseValidator,
            IValidator<SkillDTO> skillValidator)
        {
            this.courses = courses;
            this.skills = skills;
            this.materials = materials;
            this.mapper = mapper;
            this.courseValidator = courseValidator;
            this.skillValidator = skillValidator;
        }

        public string Name => "Course";

        public OperationResult AddCourse(CourseDTO course)
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

            if (this.courses.Find(x => x.Name == course.Name).SingleOrDefault() != null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseAlreadyExist";
                return result;
            }

            var courseToAdd = this.mapper.Map<CourseDTO, Course>(course);

            this.courses.Create(courseToAdd);
            this.courses.Save();

            return result;
        }

        public GetCoursesResult GetUserCourses(long userId)
        {
            var result = new GetCoursesResult();

            var userCourses = this.courses.Find(
                course => course.CreatorId == userId,
                course => course.Skills);

            result.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);
            return result;
        }

        public GetCoursesResult GetAllCourses()
        {
            var result = new GetCoursesResult();

            var userCourses = this.courses.GetAll(course => course.Skills);
            result.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);

            return result;
        }

        public OperationResult EditCourse(long userId, CourseDTO newCourseInfo)
        {
            if (newCourseInfo == null)
            {
                return new OperationResult("CourseNull", false);
            }

            var result = this.CanEditCourse(userId, newCourseInfo.Id);

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

            var courseToUpdate = this.courses.GetById(newCourseInfo.Id);
            courseToUpdate.Name = newCourseInfo.Name;
            courseToUpdate.Description = newCourseInfo.Description;

            this.courses.Update(courseToUpdate);
            this.courses.Save();

            result.MessageCode = "EditCourseSuccess";
            result.IsSuccessful = true;

            return result;
        }

        public OperationResult AddSkill(long userId, long courseId, SkillDTO skill)
        {
            if (skill == null)
            {
                return new OperationResult("SkillNull", false);
            }

            var result = this.CanEditCourse(userId, courseId);

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

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.Skills)
                .SingleOrDefault();

            var skillToAdd = this.skills.Find(sk => sk.Name == skill.Name).SingleOrDefault();

            if (skillToAdd == null)
            {
                skillToAdd = this.mapper.Map<SkillDTO, Skill>(skill);
                this.skills.Create(skillToAdd);
                this.skills.Save();
            }
            else if (course.Skills.Contains(skillToAdd))
            {
                result.IsSuccessful = false;
                result.MessageCode = "AddSkillAlreadyExists";
                return result;
            }

            course.Skills.Add(skillToAdd);

            this.courses.Update(course);
            this.courses.Save();

            result.IsSuccessful = true;
            result.MessageCode = "AddSkillSuccess";

            return result;
        }

        public OperationResult RemoveSkill(long userId, long courseId, SkillDTO skill)
        {
            if (skill == null)
            {
                return new OperationResult("SkillNull", false);
            }

            var result = this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var validationResult = this.skillValidator.Validate(skill, "Base");

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.Skills)
                .SingleOrDefault();

            var skillToRemove = course.Skills.FirstOrDefault(x => x.Name == skill.Name);

            if (skillToRemove == null)
            {
                result.MessageCode = "RemoveSkillNotFound";
                result.IsSuccessful = false;
                return result;
            }

            course.Skills.Remove(skillToRemove);
            this.courses.Update(course);
            this.courses.Save();

            result.IsSuccessful = true;
            result.MessageCode = "RemoveSkillSuccess";

            return result;
        }

        public OperationResult AddMaterialToCourse(long userId, long courseId, long materialId)
        {
            var result = this.CanEditCourse(userId, courseId);

            if (!result.IsSuccessful)
            {
                return result;
            }

            var course = this.courses.Find(
                            course => course.Id == courseId,
                            course => course.Materials)
                            .SingleOrDefault();

            if (course.Materials.Any(x => x.Id == materialId))
            {
                result.MessageCode = "AddMaterialToCourseAlreadyExists";
                result.IsSuccessful = false;
                return result;
            }

            var material = this.materials.GetById(materialId);

            course.Materials.Add(material);

            this.courses.Update(course);
            this.courses.Save();

            result.IsSuccessful = true;
            result.MessageCode = "AddMaterialToCourseSuccess";

            return result;
        }

        public OperationResult CanEditCourse(long userId, long courseId)
        {
            var result = new OperationResult();

            var course = this.courses.GetById(courseId);

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

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.CompletedUsers,
                course => course.JoinedUsers,
                course => course.Creator,
                course => course.Skills)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
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

            result.CreatorName = course.Creator.Name;
            result.Skills = this.mapper.Map<Skill, SkillDTO>(course.Skills);

            result.IsSuccessful = true;

            return result;
        }
    }
}
