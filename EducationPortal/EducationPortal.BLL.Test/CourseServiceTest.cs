using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Mappers;
using EducationPortal.BLL.Services;
using EducationPortal.DAL.Entities.EF;
using EducationPortal.DAL.Repository.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using EducationPortal.BLL.Validation;
using FluentValidation.Results;
using EducationPortal.BLL.Settings;

namespace EducationPortal.BLL.Test
{
    [TestClass]
    public class CourseServiceTest
    {
        private ICourseService courseService;
        private Mock<IRepository<Course>> courseRepository;
        private Mock<IRepository<Material>> materialRepository;
        private Mock<IRepository<Skill>> skillRepository;
        private Mock<IMapper> mapper;
        private Mock<IValidator<CourseDTO>> courseValidator;
        private Mock<IValidator<SkillDTO>> skillValidator;
        private List<Material> materials = new List<Material>();
        private List<Skill> skills = new List<Skill>();
        private List<Course> courses = new List<Course>();
        private ValidationFailure failure;

        [TestInitialize]
        public void Initialize()
        {
            failure = new ValidationFailure("", "");

            courseRepository = new Mock<IRepository<Course>>();
            materialRepository = new Mock<IRepository<Material>>();
            skillRepository = new Mock<IRepository<Skill>>();
            courseRepository = new Mock<IRepository<Course>>();
            mapper = new Mock<IMapper>();
            courseValidator = new Mock<IValidator<CourseDTO>>();
            skillValidator = new Mock<IValidator<SkillDTO>>();

            courseService = new CourseService(
                courseRepository.Object,
                skillRepository.Object,
                materialRepository.Object,
                mapper.Object,
                courseValidator.Object,
                skillValidator.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            materials.Clear();
            skills.Clear();
            courses.Clear();
        }

        [TestMethod]
        public void AddCourse_Null_CourseNullErrorCode()
        {
            CourseDTO course = null;
            var result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNull");
        }

        [TestMethod]
        public void AddCourse_CourseFieldsNull_FieldNullErrorCode()
        {
            //course name is null

            CourseDTO course = new CourseDTO()
            {
                Description = "TestDescripion",
            };

            failure.ErrorCode = "CourseNameLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            var result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNameLength");


            //course description is null

            course.Name = "TestName";
            course.Description = null;

            failure.ErrorCode = "CourseDescriptionLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseDescriptionLength");
        }

        [TestMethod]
        public void AddCourse_CourseFieldsEmpty_FieldEmptyErrorCode()
        {
            //course name is empty

            CourseDTO course = new CourseDTO()
            {
                Name = string.Empty,
                Description = "TestDescripion",
            };

            failure.ErrorCode = "CourseNameLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));  

            var result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseNameLength", result.MessageCode);


            //course description is empty

            course.Name = "TestName";
            course.Description = string.Empty;

            failure.ErrorCode = "CourseDescriptionLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseDescriptionLength", result.MessageCode);
        }

        [TestMethod]
        public void AddCourse_InvalidName_NameLengthErrorCode()
        {
            //course name is too short

            CourseDTO course = new CourseDTO()
            {
                Name = TestHelper.GenerateString(DataSettings.CourseNameMinCharacterCount - 1),
                Description = "TestDescripion",
            };

            failure.ErrorCode = "CourseNameLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            var result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseNameLength", result.MessageCode);


            //course name is too long

            course.Name = TestHelper.GenerateString(DataSettings.CourseNameMaxCharacterCount + 1);
            course.Description = string.Empty;

            result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseNameLength", result.MessageCode);
        }

        [TestMethod]
        public void AddCourse_InvalidDescription_DescriptionLengthErrorCode()
        {
            //course description is too short

            CourseDTO course = new CourseDTO()
            {
                Name = "TestName",
                Description = TestHelper.GenerateString(DataSettings.CourseDescriptionMinCharacterCount - 1),
            };

            failure.ErrorCode = "CourseDescriptionLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            var result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseDescriptionLength", result.MessageCode);


            //course description is too long

            course.Name = TestHelper.GenerateString(DataSettings.CourseDescriptionMaxCharacterCount + 1);
            course.Description = string.Empty;

            result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseDescriptionLength", result.MessageCode);
        }

        [TestMethod]
        public void AddCourse_CourseNameUsed_CourseAlreadyExistErrorCode()
        {
            courses.Add(new Course()
            {
                Name = "TestName",
                Description = "SomeDescription"
            });

            CourseDTO course = new CourseDTO()
            {
                Name = "TestName",
                Description = "TestDescription",
            };

            failure.ErrorCode = "CourseAlreadyExist";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>()))
                .Returns<Expression<Func<Course, bool>>>(x => courses.Where(x.Compile()));

            var result = courseService.AddCourse(course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseAlreadyExist", result.MessageCode);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>()));
        }

        [TestMethod]
        public void AddCourse_ValidCourse_SuccessfulResult()
        {
            CourseDTO course = new CourseDTO()
            {
                Name = "TestName",
                Description = "TestDescription",
            };

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            courseRepository
                .Setup(x => x.Create(It.IsAny<Course>()));

            courseRepository
                .Setup(x => x.Save());

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>()))
                .Returns<Expression<Func<Course, bool>>>(x => courses.Where(x.Compile()));

            mapper
                .Setup(x => x.Map<CourseDTO, Course>(It.IsAny<CourseDTO>()));

            var result = courseService.AddCourse(course);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual("AddCourseSuccess", result.MessageCode);
            mapper.Verify(x => x.Map<CourseDTO, Course>(It.IsAny<CourseDTO>()), Times.Once);
            courseRepository.Verify(x => x.Create(It.IsAny<Course>()), Times.Once);
            courseRepository.Verify(x => x.Save(), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>()));
        }

        [TestMethod]
        public void EditCourse_CourseNull_CourseNullErrorCore()
        {
            CourseDTO course = null;

            var result = courseService.EditCourse(0, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseNull", result.MessageCode);
        }

        [TestMethod]
        public void EditCourse_InvalidCourseId_NotAuthorErrorCode()
        {
            courses.Add(new Course()
            {
                Id = 1
            });

            var course = new CourseDTO()
            {
                Id = 2
            };

            courseRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns<long>(x => courses.SingleOrDefault(y => y.Id == x));

            var result = courseService.EditCourse(1, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseNotFound", result.MessageCode);
            courseRepository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [TestMethod]
        public void EditCourse_NotCreatorUserId_NotAuthorErrorCode()
        {
            courses.Add(new Course()
            {
                Id = 1,
                CreatorId = 1,
            });

            var course = new CourseDTO()
            {
                Id = 1
            };

            courseRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns<long>(x => courses.SingleOrDefault(y => y.Id == x));

            var result = courseService.EditCourse(2, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CanEditCourseNotAnAuthor", result.MessageCode);
            courseRepository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [TestMethod]
        public void EditCourse_CourseFieldsNull_FieldNullErrorCode()
        {
            //course name is null

            courses.Add(new Course()
            {
                Id = 1,
                CreatorId = 1,
            });

            var course = new CourseDTO()
            {
                Id = 1,
                Description = "TestDescription",
            };

            failure.ErrorCode = "CourseNameLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            courseRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns<long>(x => courses.SingleOrDefault(y => y.Id == x));

            var result = courseService.EditCourse(1, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNameLength");
            courseValidator.Verify(x => x.Validate(It.IsAny<CourseDTO>()), Times.Once);

            //course description is null

            course.Name = "TestName";
            course.Description = null;

            failure.ErrorCode = "CourseDescriptionLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = courseService.EditCourse(1, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseDescriptionLength");
            courseValidator.Verify(x => x.Validate(It.IsAny<CourseDTO>()), Times.Exactly(2));
        }

        [TestMethod]
        public void EditCourse_CourseFieldsEmpty_FieldEmptyErrorCode()
        {
            //course name is empty

            courses.Add(new Course()
            {
                Id = 1,
                CreatorId = 1,
            });

            var course = new CourseDTO()
            {
                Id = 1,
                Name = string.Empty,
                Description = "TestDescription",
            };

            failure.ErrorCode = "CourseNameLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            courseRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns<long>(x => courses.SingleOrDefault(y => y.Id == x));

            var result = courseService.EditCourse(1, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNameLength");
            courseValidator.Verify(x => x.Validate(It.IsAny<CourseDTO>()), Times.Once);

            //course description is empty

            course.Name = "TestName";
            course.Description = string.Empty;

            failure.ErrorCode = "CourseDescriptionLength";

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = courseService.EditCourse(1, course);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseDescriptionLength");
            courseValidator.Verify(x => x.Validate(It.IsAny<CourseDTO>()), Times.Exactly(2));
        }

        [TestMethod]
        public void EditCourse_ValidData_SuccessfulResult()
        {
            courses.Add(new Course()
            {
                Id = 1,
                CreatorId = 1,
            });

            var course = new CourseDTO()
            {
                Id = 1,
                Name = "TestName",
                Description = "TestDescription",
            };

            courseValidator
                .Setup(x => x.Validate(It.IsAny<CourseDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            courseRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns<long>(x => courses.SingleOrDefault(y => y.Id == x));

            courseRepository
                .Setup(x => x.Update(It.IsAny<Course>()));

            courseRepository
                .Setup(x => x.Save());

            var result = courseService.EditCourse(1, course);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual("EditCourseSuccess", result.MessageCode);
            courseValidator.Verify(x => x.Validate(It.IsAny<CourseDTO>()), Times.Once);
            courseRepository.Verify(x => x.Update(It.IsAny<Course>()), Times.Once);
            courseRepository.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void AddSkill_SkillNull_SkillNullErrorCode()
        {
            SkillDTO skill = null;

            var result = courseService.AddSkill(1, 1, skill);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("SkillNull", result.MessageCode);
        }

        [TestMethod]
        public void AddSkill_SkillNameNull_SkillNameNullErrorCode()
        {
            courses.Add(new Course()
            {
                Id = 1,
                CreatorId = 1,
            });

            SkillDTO skill = new SkillDTO();

            failure.ErrorCode = "SkillNameNull";

            skillValidator
                .Setup(x => x.Validate(It.IsAny<SkillDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            courseRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns<long>(x => courses.SingleOrDefault(y => y.Id == x));

            var result = courseService.AddSkill(1, 1, skill);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("SkillNameNull", result.MessageCode);
        }
    }
}
