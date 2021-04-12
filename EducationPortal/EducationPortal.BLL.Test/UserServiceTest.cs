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

namespace EducationPortal.BLL.Test
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService userService;
        private Mock<IRepository<User>> userRepository;
        private Mock<IRepository<Account>> accountRepository;
        private Mock<IRepository<Skill>> skillRepository;
        private Mock<IRepository<Course>> courseRepository;
        private Mock<IMapper> mapper;
        private Mock<IValidator<AccountDTO>> accountValidator;
        private Mock<IValidator<UserDTO>> userValidator;
        private List<User> users = new List<User>();
        private List<Account> accounts = new List<Account>();
        private List<Skill> skills = new List<Skill>();
        private List<Course> courses = new List<Course>();
        private ValidationFailure failure;

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new Mock<IRepository<User>>();
            accountRepository = new Mock<IRepository<Account>>();
            skillRepository = new Mock<IRepository<Skill>>();
            courseRepository = new Mock<IRepository<Course>>();
            mapper = new Mock<IMapper>();
            accountValidator = new Mock<IValidator<AccountDTO>>();
            userValidator = new Mock<IValidator<UserDTO>>();

            failure = new ValidationFailure("", "");

            userService = new UserService(
                userRepository.Object,
                accountRepository.Object,
                skillRepository.Object,
                courseRepository.Object,
                mapper.Object,
                accountValidator.Object,
                userValidator.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            users.Clear();
            accounts.Clear();
            skills.Clear();
            courses.Clear();
        }

        [TestMethod]
        public void Authorize_AccountNull()
        {
            AccountDTO accountDTO = null;

            var result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("AccountNull", result.MessageCode);
            Assert.IsNull(result.User);
        }

        [TestMethod]
        public void Authorize_FieldIsNull()
        {
            //password is null

            AccountDTO accountDTO = new AccountDTO()
            {
                Email = "TestEmail",
                Login = "TestLogin",
            };

            failure.ErrorCode = "AccountPasswordNull";

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            var result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "AccountPasswordNull");
            Assert.IsNull(result.User);


            //email is null

            accountDTO.Email = null;
            accountDTO.Password = "TestPassword";

            failure.ErrorCode = "AccountEmailNull";

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "AccountEmailNull");
            Assert.IsNull(result.User);


            //login is null

            accountDTO.Login = null;
            accountDTO.Email = "TestEmail";

            failure.ErrorCode = "AccountLoginNull";

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "AccountLoginNull");
            Assert.IsNull(result.User);
        }

        [TestMethod]
        public void Authorize_FieldIsEmpty()
        {
            //password is empty

            AccountDTO accountDTO = new AccountDTO()
            {
                Email = "TestEmail",
                Login = "TestLogin",
                Password = string.Empty,
            };

            failure.ErrorCode = "AccountPasswordNull";

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            var result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "AccountPasswordNull");
            Assert.IsNull(result.User);


            //email is empty

            accountDTO.Email = string.Empty;
            accountDTO.Password = "TestPassword";

            failure.ErrorCode = "AccountEmailNull";

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "AccountEmailNull");
            Assert.IsNull(result.User);


            //login is empty

            accountDTO.Login = string.Empty;
            accountDTO.Email = "TestEmail";

            failure.ErrorCode = "AccountLoginNull";

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    failure
                }));

            result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "AccountLoginNull");
            Assert.IsNull(result.User);
        }

        [TestMethod]
        public void Register_AccountNull()
        {
            AccountDTO accountDTO = null;

            var result = userService.Authorize(accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("AccountNull", result.MessageCode);
        }

        [TestMethod]
        public void Register_UserNull()
        {
            var accountDTO = new AccountDTO();
            UserDTO userDTO = null;

            var result = userService.Register(userDTO, accountDTO);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("UserNull", result.MessageCode);
        }

        [TestMethod]
        public void Register_UsedAccountData()
        {
            accounts.Add(new Account()
            {
                Id = 1,
                Email = "testemail",
                Login = "TestLogin",
            });

            accountRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns<Expression<Func<Account, bool>>>(x => accounts.Where(x.Compile()));

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            userValidator
                .Setup(x => x.Validate(It.IsAny<UserDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            //Used email case

            var accountDto = new AccountDTO()
            {
                Email = "TestEmail",
                Login = "AnotherLogin",
                Password = "TestPassword"
            };
            
            var userDto = new UserDTO() { Name = "TestName" };

            mapper.Setup(x => x.Map<AccountDTO, Account>(It.IsAny<AccountDTO>()))
                  .Returns(new Account()
                  {
                      Email = accountDto.Email,
                      Login = accountDto.Login,
                      Password = accountDto.Password,
                  });

            var result = userService.Register(userDto, accountDto);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "RegisterEmailUsed");
            accountRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);

            //Used login case

            accountDto.Email = "AnotherEmail";
            accountDto.Login = "TestLogin";

            mapper.Setup(x => x.Map<AccountDTO, Account>(It.IsAny<AccountDTO>()))
                  .Returns(new Account()
                  {
                      Email = accountDto.Email,
                      Login = accountDto.Login,
                      Password = accountDto.Password,
                  });

            result = userService.Register(userDto, accountDto);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "RegisterLoginUsed");
            accountRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Account, bool>>>()), Times.Exactly(3));
        }

        [TestMethod]
        public void Register_ValidAccount()
        {
            accounts.Add(new Account()
            {
                Id = 1,
                Email = "testemail",
                Login = "TestLogin",
            });

            accountRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns<Expression<Func<Account, bool>>>(x => accounts.Where(x.Compile()));

            accountRepository.Setup(x => x.Create(It.IsAny<Account>()));
            accountRepository.Setup(x => x.Save());

            accountValidator
                .Setup(x => x.Validate(It.IsAny<AccountDTO>(), It.IsAny<string[]>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            userValidator
                .Setup(x => x.Validate(It.IsAny<UserDTO>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()));

            var accountDto = new AccountDTO()
            {
                Email = "AnotherEmail",
                Login = "AnotherLogin",
                Password = "TestPassword"
            };
            var userDto = new UserDTO() { Name = "TestName" };

            mapper.Setup(x => x.Map<AccountDTO, Account>(It.IsAny<AccountDTO>()))
                  .Returns(new Account()
                  {
                      Email = accountDto.Email,
                      Login = accountDto.Login,
                      Password = accountDto.Password,
                  });

            mapper.Setup(x => x.Map<UserDTO, User>(It.IsAny<UserDTO>()))
                  .Returns(new User()
                  {
                      Name = userDto.Name,
                  });

            var result = userService.Register(userDto, accountDto);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "RegisterSuccess");
            accountRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Account, bool>>>()), Times.Exactly(2));
            accountRepository.Verify(x => x.Create(It.IsAny<Account>()));
            accountRepository.Verify(x => x.Save());
        }

        [TestMethod]
        public void GetUserById_InvalidUserId()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                CompletedCourses = new List<UserCompletedCourses>(),
                JoinedCourses = new List<UserJoinedCourses>(),
                UserSkills = new List<UserSkills>(),
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            var result = userService.GetUserById(3);

            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNull(result.JoinedCoursesProgress);
            Assert.IsNull(result.CompletedCourses);
            Assert.IsNull(result.SkillLevels);
            Assert.IsNull(result.User);
            Assert.AreEqual(result.MessageCode, "GetUserByIdNotFound");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void GetUserById_ValidUserId()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                CompletedCourses = new List<UserCompletedCourses>(),
                JoinedCourses = new List<UserJoinedCourses>(),
                UserSkills = new List<UserSkills>(),
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()));

            skillRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Skill, bool>>>()));

            mapper.Setup(x => x.Map<User, UserDTO>(It.IsAny<User>()))
                  .Returns(new UserDTO()
                  {
                      Name = users[0].Name
                  });

            var result = userService.GetUserById(1);

            Assert.IsTrue(result.IsSuccessful);
            Assert.IsNotNull(result.JoinedCoursesProgress);
            Assert.IsNotNull(result.CompletedCourses);
            Assert.IsNotNull(result.SkillLevels);
            Assert.IsNotNull(result.User);
            Assert.AreEqual(result.User.Name, "TestName");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            skillRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Skill, bool>>>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void JoinToCourse_InvalidUserId()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                JoinedCourses = new List<UserJoinedCourses>(),
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            var result = userService.JoinToCourse(5, 1);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "UserNotFound");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void JoinToCourse_InvalidCourseId()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                JoinedCourses = new List<UserJoinedCourses>(),
            });

            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.JoinToCourse(1, 5);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNotFound");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void JoinToCourse_JoinedCourse()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
            });

            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
                JoinedUsers = new List<UserJoinedCourses>()
                {
                    new UserJoinedCourses()
                    {
                        UserId = 1,
                        CourseId = 1
                    },
                },
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.JoinToCourse(1, 1);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseAlreadyJoin");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void JoinToCourse_CompletedCourse()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
            });

            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
                JoinedUsers = new List<UserJoinedCourses>(),
                CompletedUsers = new List<UserCompletedCourses>()
                {
                    new UserCompletedCourses()
                    {
                        UserId = 1,
                        CourseId = 1
                    },
                },
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.JoinToCourse(1, 1);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseAlreadyCompleted");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void JoinToCourse_ValidIds()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                JoinedCourses = new List<UserJoinedCourses>(),
            });

            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
                JoinedUsers = new List<UserJoinedCourses>(),
                CompletedUsers = new List<UserCompletedCourses>()
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            userRepository.Setup(x => x.Update(It.IsAny<User>()));
            userRepository.Setup(x => x.Save());

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.JoinToCourse(1, 1);

            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "JoinToCourseSuccess");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            userRepository.Verify(x => x.Update(It.IsAny<User>()));
            userRepository.Verify(x => x.Save());
            
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void AddCompletedCourse_InvalidUserId()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            var result = userService.AddCompletedCourse(5, 1);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "UserNotFound");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void AddCompletedCourse_InvalidCourseId()
        {
            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                JoinedCourses = new List<UserJoinedCourses>(),
            });

            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.AddCompletedCourse(1, 5);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNotFound");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void AddCompletedCourse_NotJoinedCourse()
        {
            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
            });

            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                JoinedCourses = new List<UserJoinedCourses>(),
                CompletedCourses = new List<UserCompletedCourses>(),
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.AddCompletedCourse(1, 1);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(result.MessageCode, "CourseNotJoined");
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }

        [TestMethod]
        public void AddCompletedCourse_CompletedCourse()
        {
            courses.Add(new Course()
            {
                Id = 1,
                Name = "TestCourse",
                Description = "TestDescription",
            });

            users.Add(new User()
            {
                Id = 1,
                Name = "TestName",
                JoinedCourses = new List<UserJoinedCourses>(),
                CompletedCourses = new List<UserCompletedCourses>()
                {
                    new UserCompletedCourses()
                    {
                        UserId = 1,
                        Course = courses[0]
                    },
                },
            });

            userRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns<Expression<Func<User, bool>>, Expression<Func<User, object>>[]>((x, y) => users.Where(x.Compile()));

            courseRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()))
                .Returns<Expression<Func<Course, bool>>, Expression<Func<Course, object>>[]>((x, y) => courses.Where(x.Compile()));

            var result = userService.AddCompletedCourse(1, 1);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("CourseAlreadyCompleted", result.MessageCode);
            userRepository.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()), Times.Once);
            courseRepository.Verify(x => x.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Expression<Func<Course, object>>[]>()), Times.Once);
        }
    }
}
