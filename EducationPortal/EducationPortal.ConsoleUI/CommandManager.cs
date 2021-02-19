namespace EducationPortal.ConsoleUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Commands;
    using EducationPortal.ConsoleUI.Validation;

    public class CommandManager
    {
        private IDictionary<string, IService> services;
        private IDictionary<string, IValidator> validators;
        private IDictionary<string, CommandInfo> commandsInfo;
        private string[] commandParts;
        private List<string> output;
        private DTOBuilder dtoBuilder;
        private Client client;
        private string consoleStatePrefix;
        private CourseDTO selectedCourse;
        private bool exitFlag;

        public CommandManager(IEnumerable<IService> services, IEnumerable<IValidator> validators)
        {
            this.dtoBuilder = DTOBuilder.GetInstance();
            this.consoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
            this.output = new List<string>();

            this.services = new Dictionary<string, IService>();

            foreach (var item in services)
            {
                this.services.Add(item.Name, item);
            }

            this.validators = new Dictionary<string, IValidator>();

            foreach (var item in validators)
            {
                this.validators.Add(item.Name, item);
            }

            this.commandsInfo = new Dictionary<string, CommandInfo>()
            {
                {
                    "reg", new CommandInfo()
                        {
                          ParamsCount = 4,
                          Description = "reg [email] [login] [password] [username]\nРегистрация нового пользователя\n",
                          Handler = this.Register,
                        }
                },
                {
                    "login", new CommandInfo()
                              {
                                ParamsCount = 2,
                                Description = "login [login | email] [password]\nАвторизация пользователя\n",
                                Handler = this.LogIn,
                              }
                },
                {
                    "logout", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "logout\nВыход из системы\n",
                                Handler = this.LogOut,
                              }
                },
                {
                    "help", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "help\nВывод списка команд\n",
                                Handler = this.CallHelp,
                              }
                },
                {
                    "exit", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "exit\nВыход из программы\n",
                                Handler = this.Exit,
                              }
                },
                {
                    "createcourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "createcourse\nСоздание нового курса\n",
                                Handler = this.CreateCourse,
                              }
                },
                {
                    "mycourses", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "mycourses\nОтобразить список созданных вами курсов\n",
                                Handler = this.GetUserCourses,
                              }
                },
                {
                    "allcourses", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "allcourses\nОтобразить список всех курсов\n",
                                Handler = this.GetAllCourses,
                              }
                },
                {
                    "entercourse", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "entercourse [number]\nВойти в выбранный курс\n",
                                Handler = this.EnterCourse,
                              }
                },
                {
                    "editcourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "editcourse\nРедактирование выбранного курса\n",
                                Handler = this.EditCourse,
                              }
                },
                {
                    "addskill", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "addskill\nДобавление умения к выбранному курсу\n",
                                Handler = this.AddSkill,
                              }
                },
                {
                    "removeskill", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "removeskill\nУдаление умения из выбранного курса\n",
                                Handler = this.RemoveSkill,
                              }
                },
                {
                    "courseinfo", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "courseinfo\nОтобразить информацию о выбранном курсе\n",
                                Handler = this.ShowCourseInfo,
                              }
                },
                {
                    "addmaterial", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "addmaterial [-new | -exist]\nДобавить материал к выбранному курсу\n",
                                Handler = this.AddMaterial,
                              }
                },
                {
                    "leavecourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "leavecourse\nВыйти из выбранного курса\n",
                                Handler = this.LeaveCourse,
                              }
                },
                {
                    "joincourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "joincourse\nНачать прохождение курса\n",
                                Handler = this.JoinCourse,
                              }
                },
                {
                    "myinfo", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "myinfo\nОтобразить личную информацию \n",
                                Handler = this.ShowUserInfo,
                              }
                },
                {
                    "nextstep", new CommandInfo()
                        {
                            ParamsCount = 0,
                            Description = "nextstep\nПерейти к следующему материалу курса\n",
                            Handler = this.DoNextStep,
                        }
                },
                {
                    "joinedcourses", new CommandInfo()
                                    {
                                        ParamsCount = 0,
                                        Description = "joinedcourses\nОтобразить список курсов, в которых вы участвуете\n",
                                        Handler = this.GetJoinedCourses,
                                    }
                },
                {
                    "completedcourses", new CommandInfo()
                                    {
                                        ParamsCount = 0,
                                        Description = "completedcourses\nОтобразить список завершенных вами курсов \n",
                                        Handler = this.GetCompletedCourses,
                                    }
                },
                {
                    "finish", new CommandInfo()
                            {
                                ParamsCount = 0,
                                Description = "finish\nПодтвердить завершение курса\n",
                                Handler = this.FinishCourse,
                            }
                },
            };
        }

        private bool IsLoggedIn { get => this.client != null; }

        public void Run()
        {
            this.exitFlag = false;
            while (!this.exitFlag)
            {
                foreach (var item in this.output)
                {
                    Console.WriteLine(item);
                }

                this.output.Clear();

                Console.Write("\n{0}> ", this.consoleStatePrefix);
                var inputStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputStr))
                {
                    continue;
                }

                this.commandParts = inputStr
                                      .Trim()
                                      .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var commandStr = this.commandParts.First();
                this.commandParts = this.commandParts.Skip(1).ToArray();

                if (this.commandsInfo.ContainsKey(commandStr))
                {
                    if (this.commandsInfo[commandStr].ParamsCount == this.commandParts.Length)
                    {
                        this.commandsInfo[commandStr].Handler();
                    }
                    else
                    {
                        this.output.Add(ConsoleMessages.ErrorWrongParamsCount);
                    }
                }
                else
                {
                    this.output.Add(string.Format(ConsoleMessages.ErrorUnknownCommand, commandStr));
                }
            }
        }

        private void SetCourseMode(CourseDTO selectedCourse)
        {
            this.selectedCourse = selectedCourse;
            this.consoleStatePrefix = ConsoleMessages.CoursePrefix + selectedCourse.Name;
        }

        private void ResetCourseMode()
        {
            this.selectedCourse = null;
            this.client.CourseCache = null;
            this.consoleStatePrefix = ConsoleMessages.UserPrefix + this.client.Info.Name;
        }

        private bool TryEditCourse()
        {
            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                this.output.Add(checkResponse.Message);
            }

            return checkResponse.IsSuccessful;
        }

        private void PrintCourses(CourseDTO[] courses)
        {
            for (int i = 0; i < courses.Length; i++)
            {
                this.output.Add(string.Format("{0}. {1}\n{2}\nУмения: {3}\n", i + 1, courses[i].Name, courses[i].Description, string.Join(", ", courses[i].Skills.Select(a => a.Name))));
            }

            this.client.CourseCache = courses;
        }

        private void PrintMaterials(MaterialDTO[] materials)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                Console.WriteLine("{0}. {1}\n", i + 1, materials[i].ToString());
            }
        }

        #region ConsoleCommandsHandlers

        private void Register()
        {
            if (this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryRegWhileLoggedIn);
                return;
            }

            var account = this.dtoBuilder.GetAccount(this.commandParts, true);
            var user = this.dtoBuilder.GetUser(this.commandParts.Skip(typeof(AccountDTO).GetProperties().Length).ToArray());
            var command = new RegisterCommand(this.services["User"] as IUserService, this.validators["Register"], user, account);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);
        }

        private void LogIn()
        {
            if (this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryLogInWhileLoggedIn);
                return;
            }

            var account = this.dtoBuilder.GetAccount(this.commandParts.ToArray());
            var command = new AuthorizeCommand(this.services["User"] as IUserService, account);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);

            if (response.IsSuccessful)
            {
                this.client = Client.GetInstance();
                this.client.Id = response.Id;
                this.client.Info = response.User;
                this.consoleStatePrefix = ConsoleMessages.UserPrefix + this.client.Info.Name;
            }
        }

        private void LogOut()
        {
            if (this.IsLoggedIn)
            {
                this.ResetCourseMode();
                this.client = null;
                this.consoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
                this.output.Add(ConsoleMessages.InfoLoggedOut);
            }
            else
            {
                this.output.Add(ConsoleMessages.ErrorTryLogOutWhileLoggedOut);
            }
        }

        private void CallHelp()
        {
            this.output.AddRange(this.commandsInfo.Select(command => command.Value.Description));
        }

        private void Exit()
        {
            this.exitFlag = true;
        }

        private void CreateCourse()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            Console.WriteLine("Введите название курса: ");
            var name = Console.ReadLine();
            Console.WriteLine("Введите описание курса: ");
            var description = Console.ReadLine();

            var course = new CourseDTO()
            {
                Name = name,
                Description = description,
                CreatorId = this.client.Id,
            };

            var command = new AddCourseCommand(this.services["Course"] as ICourseService, this.validators["Course"], course);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);
        }

        private void GetAllCourses()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse != null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetAllCoursesCommand(this.services["Course"] as ICourseService);
            command.Execute();
            var response = command.Response;

            var allCourses = response.Courses.ToArray();
            this.PrintCourses(allCourses);
        }

        private void GetUserCourses()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse != null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetUserCoursesCommand(this.services["Course"] as ICourseService, this.client.Id);
            command.Execute();
            var response = command.Response;

            var myCourses = response.Courses.ToArray();
            this.PrintCourses(myCourses);
        }

        private void EnterCourse()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse != null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            if (this.client.CourseCache == null)
            {
                this.output.Add(ConsoleMessages.ErrorCourseListIsEmpty);
                return;
            }

            var isNumberCorrect = long.TryParse(this.commandParts[0], out long number);

            if (!isNumberCorrect
             || number - 1 < 0
             || number - 1 >= this.client.CourseCache.Length)
            {
                this.output.Add(ConsoleMessages.ErrorIncorrectNumberOfCourse);
                return;
            }

            this.SetCourseMode(this.client.CourseCache[number - 1]);
        }

        private void EditCourse()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            if (!this.TryEditCourse())
            {
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                this.output.Add(checkResponse.Message);
                return;
            }

            Console.WriteLine(ConsoleMessages.InputCourseName);
            var name = Console.ReadLine();
            Console.WriteLine(ConsoleMessages.InputCourseDescription);
            var description = Console.ReadLine();

            var course = new CourseDTO()
            {
                Name = name,
                Description = description,
                CreatorId = this.selectedCourse.CreatorId,
                Id = this.selectedCourse.Id,
            };

            var command = new EditCourseCommand(this.services["Course"] as ICourseService, this.validators["Course"], course, this.client.Id);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);

            if (response.IsSuccessful)
            {
                this.consoleStatePrefix = ConsoleMessages.CoursePrefix + name;
                this.selectedCourse.Name = name;
                this.selectedCourse.Description = description;
            }
        }

        private void AddSkill()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            if (!this.TryEditCourse())
            {
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                this.output.Add(checkResponse.Message);
                return;
            }

            var skill = this.dtoBuilder.GetSkill(this.commandParts);

            var command = new AddSkillCommand(this.services["Course"] as ICourseService, this.validators["Skill"], this.client.Id, this.selectedCourse.Id, skill);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);

            if (response.IsSuccessful)
            {
                this.selectedCourse.Skills = this.selectedCourse.Skills.Append(skill).ToArray();
            }
        }

        private void RemoveSkill()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            if (!this.TryEditCourse())
            {
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                this.output.Add(checkResponse.Message);
                return;
            }

            var skillToRemove = this.dtoBuilder.GetSkill(this.commandParts);

            var command = new RemoveSkillCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id, skillToRemove);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);

            if (response.IsSuccessful)
            {
                this.selectedCourse.Skills = this.selectedCourse.Skills
                                                      .Where(skill => skill.Name != skillToRemove.Name)
                                                      .ToArray();
            }
        }

        private void ShowCourseInfo()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var getCourseStatusCommand = new GetCourseStatusCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            getCourseStatusCommand.Execute();
            var getCourseStatusResponse = getCourseStatusCommand.Response;

            this.output.Add(string.Format(ConsoleMessages.OutputCourseName, this.selectedCourse.Name));
            this.output.Add(string.Format(ConsoleMessages.OutputCourseAuthorName, getCourseStatusResponse.CreatorName));
            this.output.Add(string.Format(ConsoleMessages.OutputCourseDescription, this.selectedCourse.Description));

            var skillStr = this.selectedCourse.Skills.Count() != 0
                                ? string.Join(", ", this.selectedCourse.Skills.Select(a => a.Name))
                                : ConsoleMessages.OutputEmptySkillList;

            this.output.Add(string.Format(ConsoleMessages.OutputCourseSkills, skillStr));

            if (!getCourseStatusResponse.IsSuccessful)
            {
                this.output.Add(getCourseStatusResponse.Message);
                return;
            }

            if (getCourseStatusResponse.IsCreator)
            {
                this.output.Add(ConsoleMessages.OutputIsCourseAuthor);
            }

            if (getCourseStatusResponse.IsJoined)
            {
                this.output.Add(ConsoleMessages.OutputIsJoiningCourse);
            }

            if (getCourseStatusResponse.IsCompleted)
            {
                this.output.Add(ConsoleMessages.OutputHasCompleteCourse);
            }
        }

        private void AddMaterial()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            if (!this.TryEditCourse())
            {
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                this.output.Add(checkResponse.Message);
                return;
            }

            if (this.commandParts[0] == "-new")
            {
                Console.WriteLine(ConsoleMessages.InputMaterialName);
                var name = Console.ReadLine();

                Console.WriteLine(ConsoleMessages.InputMaterialUrl);
                var url = Console.ReadLine();

                Console.WriteLine("Введите тип материала (Статья | Книга | Видео)");
                var materialType = Console.ReadLine();

                MaterialDTO materialToAdd;

                switch (materialType)
                {
                    case "Статья":

                        Console.WriteLine("Введите дату публикации статьи (ГГГГ-ММ-ДД): ");
                        var date = Console.ReadLine();

                        materialToAdd = this.dtoBuilder.GetArticle(name, url, date);

                        break;

                    case "Книга":

                        Console.WriteLine("Введите имена авторов, через запятую: ");
                        var authors = Console.ReadLine();

                        Console.WriteLine("Введите количество страниц: ");
                        var pageCount = Console.ReadLine();

                        Console.WriteLine("Введите формат книги: ");
                        var format = Console.ReadLine().Trim();

                        Console.WriteLine("Введите год издания книги: ");
                        var year = Console.ReadLine();

                        materialToAdd = this.dtoBuilder.GetBook(name, url, authors, pageCount, format, year);

                        break;

                    case "Видео":

                        Console.WriteLine("Введите длительность видео (в секундах): ");
                        var duration = Console.ReadLine();

                        Console.WriteLine("Введите качество видео: ");
                        var quality = Console.ReadLine().Trim();

                        materialToAdd = this.dtoBuilder.GetVideo(name, url, duration, quality);

                        break;

                    default:
                        this.output.Add("Неверно указан тип материала!");
                        return;
                }

                var addMaterialCommand = new AddMaterialCommand(this.services["Material"] as IMaterialService, this.validators["Material"], materialToAdd);
                addMaterialCommand.Execute();
                var materialResponse = addMaterialCommand.Response;

                if (!materialResponse.IsSuccessful)
                {
                    this.output.Add(materialResponse.Message);
                    return;
                }

                materialToAdd.Id = materialResponse.MaterialId;

                var addMaterialToCourseCommand = new AddMaterialToCourseCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id, materialToAdd.Id);
                addMaterialToCourseCommand.Execute();
                var addMaterialResponse = addMaterialToCourseCommand.Response;
                this.output.Add(addMaterialResponse.Message);
            }
            else if (this.commandParts[0] == "-exist")
            {
                var getMaterialsCommand = new GetAllMaterialsCommand(this.services["Material"] as IMaterialService);
                getMaterialsCommand.Execute();
                var response = getMaterialsCommand.Response;

                if (!response.IsSuccessful)
                {
                    this.output.Add("Список материалов пуст!");
                    return;
                }

                var allMaterials = response.Materials.ToArray();
                this.PrintMaterials(allMaterials);

                Console.WriteLine("\nВведите номер материала:");
                var numberStr = Console.ReadLine();

                var isNumberCorrect = long.TryParse(numberStr, out long number);

                if (!isNumberCorrect
                 || number - 1 < 0
                 || number - 1 >= allMaterials.Length)
                {
                    this.output.Add(ConsoleMessages.ErrorIncorrectNumberOfMaterial);
                    return;
                }

                var checkMaterialExistingCommand = new CheckMaterialExistingCommmand(this.services["Material"] as IMaterialService, allMaterials[number - 1].Id);
                checkMaterialExistingCommand.Execute();
                var checkMaterialResponse = checkMaterialExistingCommand.Response;

                if (!checkMaterialResponse.IsSuccessful)
                {
                    this.output.Add(checkMaterialResponse.Message);
                    return;
                }

                var addMaterialToCourseCommand = new AddMaterialToCourseCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id, allMaterials[number - 1].Id);
                addMaterialToCourseCommand.Execute();
                var addMaterialToCourseResponse = addMaterialToCourseCommand.Response;
                this.output.Add(addMaterialToCourseResponse.Message);
            }
            else
            {
                this.output.Add(ConsoleMessages.ErrorWrongParamValue);
            }
        }

        private void LeaveCourse()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            this.ResetCourseMode();
        }

        private void JoinCourse()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var checkCourseCommand = new CheckCourseJoiningCommand(this.services["Course"] as ICourseService, this.client.Id, this.selectedCourse.Id);
            checkCourseCommand.Execute();
            var checkResponse = checkCourseCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                this.output.Add(checkResponse.Message);
                return;
            }

            var joinCourseCommand = new JoinCourseCommand(this.services["User"] as IUserService, this.client.Id, this.selectedCourse.Id);
            joinCourseCommand.Execute();
            var joinCourseResponse = joinCourseCommand.Response;

            this.output.Add(joinCourseResponse.Message);

            if (!joinCourseResponse.IsSuccessful)
            {
                return;
            }
        }

        private void ShowUserInfo()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse != null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetUserInfoCommand(this.services["User"] as IUserService, this.client.Id);
            command.Execute();
            var response = command.Response;

            if (!response.IsSuccessful)
            {
                this.output.Add(response.Message);
                return;
            }

            this.output.Add(string.Format("Профиль пользователя {0}", this.client.Info.Name));

            var userSkillStrings = response.SkillLevels.Select(sl => string.Format("{0} ({1})", sl.Key.Name, sl.Value));
            this.output.Add(string.Format("Умения: {0}", string.Join(", ", userSkillStrings)));

            var joinedCourseStrings = response.JoinedCoursesProgress.Select(cp => string.Format("\n{0} - {1}%", cp.Key.Name, cp.Value));
            this.output.Add(string.Format("Курсы в процессе: {0}", string.Join(", ", joinedCourseStrings)));

            var courseNameStrings = response.CompletedCourses.Select(cc => cc.Name);
            this.output.Add(string.Format("Завершенные курсы: {0}", string.Join(", ", courseNameStrings)));
        }

        private void DoNextStep()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var command = new GetNextMaterialCommand(this.services["User"] as IUserService, this.client.Id, this.selectedCourse.Id);
            command.Execute();
            var response = command.Response;

            if (!response.IsSuccessful)
            {
                this.output.Add(response.Message);
                return;
            }

            foreach (var item in response.Materials)
            {
                this.output.Add("Изучен новый материал!");
                this.output.Add(item.ToString());
            }
        }

        private void GetJoinedCourses()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse != null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetJoinedCoursesCommand(this.services["User"] as IUserService, this.client.Id);
            command.Execute();
            var response = command.Response;

            if (!response.IsSuccessful)
            {
                this.output.Add(response.Message);
                return;
            }

            var joinedCourses = response.Courses.ToArray();
            this.PrintCourses(joinedCourses);
        }

        private void GetCompletedCourses()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse != null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetCompletedCoursesCommand(this.services["User"] as IUserService, this.client.Id);
            command.Execute();
            var response = command.Response;

            if (!response.IsSuccessful)
            {
                this.output.Add(response.Message);
                return;
            }

            var joinedCourses = response.Courses.ToArray();
            this.PrintCourses(joinedCourses);
        }

        private void FinishCourse()
        {
            if (!this.IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.selectedCourse == null)
            {
                this.output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var completeCourseCommand = new CompleteCourseCommand(this.services["User"] as IUserService, this.client.Id, this.selectedCourse.Id);
            completeCourseCommand.Execute();
            var completeCourseResponse = completeCourseCommand.Response;

            if (!completeCourseResponse.IsSuccessful)
            {
                this.output.Add(completeCourseResponse.Message);
                return;
            }

            this.output.Add(string.Format("Курс {0} успешно завершен!", this.selectedCourse.Name));

            foreach (var skillLevelPair in completeCourseResponse.RecievedSkills)
            {
                if (skillLevelPair.Value == 1)
                {
                    this.output.Add(string.Format("Получено новое умение - \"{0}\"", skillLevelPair.Key.Name));
                }
                else
                {
                    this.output.Add(string.Format("Умение \"{0}\" повышено!", skillLevelPair.Key.Name));
                }
            }
        }

        #endregion
    }
}
