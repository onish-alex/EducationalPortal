using System;
using System.Linq;
using System.Collections.Generic;
using EducationPortal.ConsoleUI.Commands;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI
{
    public class CommandManager
    {
        private IDictionary<string, IService> services;
        private IDictionary<string, CommandInfo> commandsInfo;
        private string[] commandParts;
        private List<string> output;
        private DTOBuilder dtoBuilder;
        private Client client;
        private string consoleStatePrefix;
        private CourseDTO selectedCourse;
        private bool exitFlag;

        private class CourseProgress
        {
            public string Name { get; set; }

            public int CompletionPercent { get; set; }

            public override string ToString()
            {
                return string.Format("\n{0} - {1}%", Name, CompletionPercent);
            }
        }

        private bool IsLoggedIn { get => client != null; }
        
        public CommandManager(IEnumerable<IService> services)
        {
            this.dtoBuilder = DTOBuilder.GetInstance();
            this.consoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
            this.output = new List<string>();
            this.services = new Dictionary<string, IService>();

            foreach (var item in services)
            {
                this.services.Add(item.Name, item);
            }

            commandsInfo = new Dictionary<string, CommandInfo>()
            {
                {"reg", new CommandInfo()
                        {
                          ParamsCount = 4,
                          Description = "reg [email] [login] [password] [username]\nРегистрация нового пользователя\n",
                          Handler = Register
                        }
                },
                {"login", new CommandInfo()
                              {
                                ParamsCount = 2,
                                Description = "login [login | email] [password]\nАвторизация пользователя\n",
                                Handler = LogIn
                              }
                },
                {"logout", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "logout\nВыход из системы\n",
                                Handler = LogOut
                              }
                },
                {"help", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "help\nВывод списка команд\n",
                                Handler = CallHelp
                              }
                },
                {"exit", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "exit\nВыход из программы\n",
                                Handler = Exit
                              }
                },
                {"createcourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "createcourse\nСоздание нового курса\n",
                                Handler = CreateCourse
                              }
                },
                {"mycourses", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "mycourses\nОтобразить список созданных вами курсов\n",
                                Handler = GetUserCourses
                              }
                },
                {"allcourses", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "allcourses\nОтобразить список всех курсов\n",
                                Handler = GetAllCourses
                              }
                },
                {"entercourse", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "entercourse [number]\nВойти в выбранный курс\n",
                                Handler = EnterCourse
                              }
                },
                {"editcourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "editcourse\nРедактирование выбранного курса\n",
                                Handler = EditCourse
                              }
                },
                {"addskill", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "addskill\nДобавление умения к выбранному курсу\n",
                                Handler = AddSkill
                              }
                },
                {"removeskill", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "removeskill\nУдаление умения из выбранного курса\n",
                                Handler = RemoveSkill
                              }
                },
                {"courseinfo", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "courseinfo\nОтобразить информацию о выбранном курсе\n",
                                Handler = ShowCourseInfo
                              }
                },
                {"addmaterial", new CommandInfo()
                              {
                                ParamsCount = 1,
                                Description = "addmaterial [-new | -exist]\nДобавить материал к выбранному курсу\n",
                                Handler = AddMaterial
                              }
                },
                {"leavecourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "leavecourse\nВыйти из выбранного курса\n",
                                Handler = LeaveCourse
                              }
                },
                {"joincourse", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "joincourse\nНачать прохождение курса\n",
                                Handler = JoinCourse
                              }
                },
                {"myinfo", new CommandInfo()
                              {
                                ParamsCount = 0,
                                Description = "myinfo\nОтобразить личную информацию \n",
                                Handler = ShowUserInfo
                              }
                },
                {"nextstep", new CommandInfo()
                        {
                            ParamsCount = 0,
                            Description = "nextstep\nПерейти к следующему материалу курса\n",
                            Handler = DoNextStep
                        } 
                },
                {"joinedcourses", new CommandInfo()
                                    {
                                        ParamsCount = 0,
                                        Description = "joinedcourses\nОтобразить список курсов, в которых вы участвуете\n",
                                        Handler = GetJoinedCourses
                                    } 
                },
                {"completedcourses", new CommandInfo()
                                    {
                                        ParamsCount = 0,
                                        Description = "completedcourses\nОтобразить список завершенных вами курсов \n",
                                        Handler = GetCompletedCourses
                                    }
                },
            };
        }

        public void Run()
        {
            exitFlag = false;
            while (!exitFlag)
            {
                foreach (var item in output)
                {
                    Console.WriteLine(item);
                }

                output.Clear();

                Console.Write("\n{0}> ", consoleStatePrefix);
                var inputStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputStr))
                {
                    continue;
                }

                commandParts = inputStr
                                .Trim()
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var commandStr = commandParts.First();
                commandParts = commandParts.Skip(1).ToArray();

                if (commandsInfo.ContainsKey(commandStr))
                {
                    if (commandsInfo[commandStr].ParamsCount == commandParts.Length)
                    {
                        commandsInfo[commandStr].Handler();
                    }
                    else
                    {
                        output.Add(ConsoleMessages.ErrorWrongParamsCount);
                    }
                } 
                else
                {
                    output.Add(string.Format(ConsoleMessages.ErrorUnknownCommand, commandStr));
                }
            }
        }

        private void SetCourseMode(CourseDTO selectedCourse)
        {
            this.selectedCourse = selectedCourse;
            consoleStatePrefix = ConsoleMessages.CoursePrefix + selectedCourse.Name;     
        }

        private void ResetCourseMode()
        {
            this.selectedCourse = null;
            this.client.courseCache = null;
            consoleStatePrefix = ConsoleMessages.UserPrefix + client.Info.Name;
        }

        private void CompleteCourse()
        {
            var completeCourseCommand = new CompleteCourseCommand(this.services["User"] as IUserService, client.Id, selectedCourse);
            completeCourseCommand.Execute();
            var completeCourseResponse = completeCourseCommand.Response;

            if (!completeCourseResponse.IsSuccessful)
            {
                output.Add(completeCourseResponse.Message);
                return;
            }

            output.Add(string.Format("Курс {0} успешно завершен!", selectedCourse.Name));
            foreach (var skill in selectedCourse.Skills)
            {
                if (client.Info.Skills.Keys.Contains(skill.Name))
                {
                    client.Info.Skills[skill.Name].Level++;
                    output.Add(string.Format("Умение \"{0}\" повышено!", skill.Name));
                }
                else
                {
                    client.Info.Skills.Add(skill.Name, new UserSkillDTO() { Skill = skill, Level = 1 });
                    output.Add(string.Format("Получено новое умение - \"{0}\"", skill.Name));
                }
            }

            client.Info.CompletedCourseIds = client.Info.CompletedCourseIds.Append(selectedCourse.Id).ToArray();
            client.Info.JoinedCourseIds = client.Info.JoinedCourseIds.Except(Enumerable.Repeat(selectedCourse.Id, 1)).ToArray();
        }

        private void TakeNextMaterial(long nextMaterialId)
        {
            var getNextMaterialCommand = new GetMaterialsByIdsCommand(this.services["Material"] as IMaterialService, new long[] { nextMaterialId });
            getNextMaterialCommand.Execute();
            var getNextMaterialResponse = getNextMaterialCommand.Response;

            if (!getNextMaterialResponse.IsSuccessful)
            {
                output.Add(getNextMaterialResponse.Message);
                return;
            }

            var nextMaterial = getNextMaterialResponse.Materials.Single();

            var addLearnedMaterialCommand = new AddLearnedMaterialCommand(this.services["User"] as IUserService, client.Id, nextMaterialId);
            addLearnedMaterialCommand.Execute();
            var addLearnedMaterialResponse = addLearnedMaterialCommand.Response;

            output.Add(addLearnedMaterialResponse.Message);

            if (!addLearnedMaterialResponse.IsSuccessful)
            {
                return;
            }

            client.Info.CompletedMaterialIds = client.Info.CompletedMaterialIds.Append(nextMaterialId).ToArray();

            output.Add(nextMaterial.ToString());
        }

        private void PrintCourses(CourseDTO[] courses)
        {
            for (int i = 0; i < courses.Length; i++)
            {
                output.Add(string.Format("{0}. {1}\n{2}\n", i + 1, courses[i].Name, courses[i].Description));
            }
            client.courseCache = courses;
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
            if (IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryRegWhileLoggedIn);
                return;
            }

            var account = dtoBuilder.GetAccount(commandParts, true);
            var user = dtoBuilder.GetUser(commandParts.Skip(typeof(AccountDTO).GetProperties().Length).ToArray());
            var command = new RegisterCommand(this.services["User"] as IUserService, user, account);
            command.Execute();
            var response = command.Response;
            output.Add(response.Message);
        }

        private void LogIn()
        {
            if (IsLoggedIn)
            {
                this.output.Add(ConsoleMessages.ErrorTryLogInWhileLoggedIn);
                return;
            }

            var account = dtoBuilder.GetAccount(commandParts.ToArray());
            var command = new AuthorizeCommand(this.services["User"] as IUserService, account);
            command.Execute();
            var response = command.Response;
            this.output.Add(response.Message);

            if (response.IsSuccessful)
            {
                this.client = Client.GetInstance();
                this.client.Id = response.Id;
                this.client.Info = response.User;
                this.consoleStatePrefix = ConsoleMessages.UserPrefix + client.Info.Name;
            }
        }

        private void LogOut()
        {
            if (IsLoggedIn)
            {
                ResetCourseMode();
                this.client = null;
                consoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
                output.Add(ConsoleMessages.InfoLoggedOut);
            }
            else
            {
                output.Add(ConsoleMessages.ErrorTryLogOutWhileLoggedOut);
            }
        }

        private void CallHelp()
        {
            output.AddRange(commandsInfo.Select(command => command.Value.Description));
        }

        private void Exit()
        {
            exitFlag = true;
        }

        private void CreateCourse()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
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
                CreatorId = client.Id,
                Skills = new SkillDTO[0]
            };

            var command = new AddCourseCommand(this.services["Course"] as ICourseService, course);
            command.Execute();
            var response = command.Response;
            output.Add(response.Message);
        }

        private void GetAllCourses()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse != null)
            {
                output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetAllCoursesCommand(this.services["Course"] as ICourseService);
            command.Execute();
            var response = command.Response;

            var allCourses = response.Courses.ToArray();
            PrintCourses(allCourses);
        }

        private void GetUserCourses()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse != null)
            {
                output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetUserCoursesCommand(this.services["Course"] as ICourseService, client.Id);
            command.Execute();
            var response = command.Response;

            var myCourses = response.Courses.ToArray();
            PrintCourses(myCourses);
        }

        private void EnterCourse()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse != null)
            {
                output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            if (client.courseCache == null)
            {
                output.Add(ConsoleMessages.ErrorCourseListIsEmpty);
                return;
            }

            var isNumberCorrect = long.TryParse(commandParts[0], out long number);

            if (!isNumberCorrect
             || number - 1 < 0
             || number - 1 >= client.courseCache.Length)
            {
                output.Add(ConsoleMessages.ErrorIncorrectNumberOfCourse);
                return;
            }

            SetCourseMode(client.courseCache[number - 1]);
        }

        private void EditCourse()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                output.Add(checkResponse.Message);
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
                CreatorId = selectedCourse.CreatorId,
                Id = selectedCourse.Id,
            };

            var command = new EditCourseCommand(this.services["Course"] as ICourseService, course, client.Id);
            command.Execute();
            var response = command.Response;
            output.Add(response.Message);

            if (response.IsSuccessful)
            {
                consoleStatePrefix = ConsoleMessages.CoursePrefix + name;
                selectedCourse.Name = name;
                selectedCourse.Description = description;
            }
        }

        private void AddSkill()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                output.Add(checkResponse.Message);
                return;
            }

            var skill = dtoBuilder.GetSkill(commandParts);

            var command = new AddSkillCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id, skill);
            command.Execute();
            var response = command.Response;
            output.Add(response.Message);

            if (response.IsSuccessful)
            {
                selectedCourse.Skills = selectedCourse.Skills.Append(skill).ToArray();
            }
        }

        private void RemoveSkill()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                output.Add(checkResponse.Message);
                return;
            }

            var skillToRemove = dtoBuilder.GetSkill(commandParts);

            var command = new RemoveSkillCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id, skillToRemove);
            command.Execute();
            var response = command.Response;
            output.Add(response.Message);

            if (response.IsSuccessful)
            {
                selectedCourse.Skills = selectedCourse.Skills
                                                      .Where(skill => skill.Name != skillToRemove.Name)
                                                      .ToArray();
            }
        }

        private void ShowCourseInfo()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var command = new GetUserCommand(this.services["User"] as IUserService, selectedCourse.CreatorId);
            command.Execute();
            var response = command.Response;

            output.Add(string.Format(ConsoleMessages.OutputCourseName, selectedCourse.Name));
            output.Add(string.Format(ConsoleMessages.OutputCourseAuthorName, (response.IsSuccessful) ? response.User.Name : response.Message));
            output.Add(string.Format(ConsoleMessages.OutputCourseDescription, selectedCourse.Description));

            output.Add(string.Format(ConsoleMessages.OutputCourseSkills, (selectedCourse.Skills.Length != 0)
                                                                            ? string.Join(", ", selectedCourse.Skills.Select(a => a.Name))
                                                                            : ConsoleMessages.OutputEmptySkillList));

            if (client.Id == selectedCourse.CreatorId)
            {
                output.Add(ConsoleMessages.OutputIsCourseAuthor);
            }

            if (client.Info.JoinedCourseIds.Contains(selectedCourse.Id))
            {
                output.Add(ConsoleMessages.OutputIsJoiningCourse);
            }

            if (client.Info.CompletedCourseIds.Contains(selectedCourse.Id))
            {
                output.Add(ConsoleMessages.OutputHasCompleteCourse);
            }
        }

        private void AddMaterial()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var checkCommand = new CheckCourseEditingCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id);
            checkCommand.Execute();
            var checkResponse = checkCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                output.Add(checkResponse.Message);
                return;
            }

            if (commandParts[0] == "-new")
            {
                MaterialDTO materialToAdd = null;

                Console.WriteLine(ConsoleMessages.InputMaterialName);
                var name = Console.ReadLine();

                Console.WriteLine(ConsoleMessages.InputMaterialUrl);
                var url = Console.ReadLine();

                Console.WriteLine("Введите тип материала (Статья | Книга | Видео)");
                var materialType = Console.ReadLine();

                switch (materialType)
                {
                    case "Статья":

                        Console.WriteLine("Введите дату публикации статьи (ГГГГ-ММ-ДД): ");
                        var date = Console.ReadLine();

                        materialToAdd = dtoBuilder.GetArticle(name, url, date);

                        break;

                    case "Книга":

                        Console.WriteLine("Введите имена авторов, через запятую: ");
                        var authors = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(str => str.Trim())
                                                        .ToArray();

                        Console.WriteLine("Введите количество страниц: ");
                        var pageCount = Console.ReadLine();

                        Console.WriteLine("Введите формат книги: ");
                        var format = Console.ReadLine().Trim();

                        Console.WriteLine("Введите год издания книги: ");
                        var year = Console.ReadLine();

                        materialToAdd = dtoBuilder.GetBook(name, url, authors, pageCount, format, year);

                        break;

                    case "Видео":

                        Console.WriteLine("Введите длительность видео (в секундах): ");
                        var duration = Console.ReadLine();

                        Console.WriteLine("Введите качество видео: ");
                        var quality = Console.ReadLine().Trim();

                        materialToAdd = dtoBuilder.GetVideo(name, url, duration, quality);

                        break;

                    default:
                        output.Add("Неверно указан тип материала!");
                        return;
                }

                var addMaterialCommand = new AddMaterialCommand(this.services["Material"] as IMaterialService, materialToAdd);
                addMaterialCommand.Execute();
                var materialResponse = addMaterialCommand.Response;

                if (!materialResponse.IsSuccessful)
                {
                    output.Add(materialResponse.Message);
                    return;
                }

                var addMaterialToCourseCommand = new AddMaterialToCourseCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id, materialResponse.MaterialId);
                addMaterialToCourseCommand.Execute();
                var addMaterialResponse = addMaterialToCourseCommand.Response;
                output.Add(addMaterialResponse.Message);
            }
            else if (commandParts[0] == "-exist")
            {
                var getMaterialsCommand = new GetAllMaterialsCommand(this.services["Material"] as IMaterialService);
                getMaterialsCommand.Execute();
                var response = getMaterialsCommand.Response;

                if (!response.IsSuccessful)
                {
                    output.Add("Список материалов пуст!");
                    return;
                }

                var allMaterials = response.Materials.ToArray();
                PrintMaterials(allMaterials);

                Console.WriteLine("\nВведите номер материала:");
                var numberStr = Console.ReadLine();

                var isNumberCorrect = long.TryParse(numberStr, out long number);

                if (!isNumberCorrect
                 || number - 1 < 0
                 || number - 1 >= allMaterials.Length)
                {
                    output.Add(ConsoleMessages.ErrorIncorrectNumberOfMaterial);
                    return;
                }

                var addMaterialToCourseCommand = new AddMaterialToCourseCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id, allMaterials[number - 1].Id);
                addMaterialToCourseCommand.Execute();
                var addMaterialToCourseResponse = addMaterialToCourseCommand.Response;
                output.Add(addMaterialToCourseResponse.Message);
            }
            else
            {
                output.Add(ConsoleMessages.ErrorWrongParamValue);
            }
        }

        private void LeaveCourse()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            ResetCourseMode();
        }

        private void JoinCourse()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            if (client.Info.CompletedCourseIds.Contains(selectedCourse.Id))
            {
                output.Add(ConsoleMessages.ErrorAlreadyCompleteCourse);
                return;
            }

            var checkCourseCommand = new CheckCourseJoiningCommand(this.services["Course"] as ICourseService, client.Id, selectedCourse.Id);
            checkCourseCommand.Execute();
            var checkResponse = checkCourseCommand.Response;

            if (!checkResponse.IsSuccessful)
            {
                output.Add(checkResponse.Message);
                return;
            }

            var joinCourseCommand = new JoinCourseCommand(this.services["User"] as IUserService, client.Id, selectedCourse.Id);
            joinCourseCommand.Execute();
            var joinCourseResponse = joinCourseCommand.Response;

            output.Add(joinCourseResponse.Message);

            if (!joinCourseResponse.IsSuccessful)
            {
                return;
            }

            client.Info.JoinedCourseIds = client.Info.JoinedCourseIds.Append(selectedCourse.Id).ToArray();
        }

        private void ShowUserInfo()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse != null)
            {
                output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var getJoinedCoursesCommand = new GetCoursesByIdsCommand(this.services["Course"] as ICourseService, client.Info.JoinedCourseIds);
            getJoinedCoursesCommand.Execute();
            var getJoinedCoursesResponse = getJoinedCoursesCommand.Response;
            
            var getCompletedCoursesCommand = new GetCoursesByIdsCommand(this.services["Course"] as ICourseService, client.Info.CompletedCourseIds);
            getCompletedCoursesCommand.Execute();
            var getCompletedCoursesResponse = getCompletedCoursesCommand.Response;

            var coursesProgress = new List<CourseProgress>();
            foreach (var course in getJoinedCoursesResponse.Courses)
            {
                var completedMaterialCount = client.Info.CompletedMaterialIds
                                                        .Intersect(course.MaterialIds)
                                                        .Count();

                var allMaterialCount = course.MaterialIds
                                             .Count();

                var percent = (allMaterialCount != 0)
                    ? Math.Round(completedMaterialCount / (double)allMaterialCount, 2)
                    : 0;
                
                coursesProgress.Add(
                    new CourseProgress() 
                    { 
                        Name = course.Name, 
                        CompletionPercent = (int)(percent * 100) 
                    }); 
            }

            output.Add(string.Format("Профиль пользователя {0}", client.Info.Name));

            var userSkillStrings = client.Info.Skills.Select(us => us.ToString());
            output.Add(string.Format("Умения: {0}", string.Join(", ", userSkillStrings)));

            var joinedCourseStrings = coursesProgress.Select(cp => cp.ToString());
            output.Add(string.Format("Курсы в процессе: {0}", string.Join(", ", joinedCourseStrings)));

            var courseNameStrings = getCompletedCoursesResponse.Courses.Select(course => course.Name);
            output.Add(string.Format("Завершенные курсы: {0}", string.Join(", ", courseNameStrings)));
        }

        private void DoNextStep()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse == null)
            {
                output.Add(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            if (!client.Info.JoinedCourseIds.Contains(selectedCourse.Id))
            {
                output.Add(ConsoleMessages.ErrorNotJoiningCourse);
                return;
            }

            var getCurrentCourseCommand = new GetCoursesByIdsCommand(this.services["Course"] as ICourseService, new long[] { selectedCourse.Id });
            getCurrentCourseCommand.Execute();
            var currentCourseResponse = getCurrentCourseCommand.Response;
            selectedCourse = currentCourseResponse.Courses.Single();

            long nextMaterialId = -1;

            for (int i = 0; i < selectedCourse.MaterialIds.Length; i++)
            {
                if (!client.Info.CompletedMaterialIds.Contains(selectedCourse.MaterialIds[i]))
                {
                    nextMaterialId = selectedCourse.MaterialIds[i];
                    break;
                } 
            }

            if (nextMaterialId == -1)
            {
                CompleteCourse();
            }
            else 
            {
                TakeNextMaterial(nextMaterialId);
            }
        }

        private void GetJoinedCourses()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse != null)
            {
                output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetCoursesByIdsCommand(this.services["Course"] as ICourseService, client.Info.JoinedCourseIds);
            command.Execute();
            var response = command.Response;

            var joinedCourses = response.Courses.ToArray();
            PrintCourses(joinedCourses);
        }

        private void GetCompletedCourses()
        {
            if (!IsLoggedIn)
            {
                output.Add(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (selectedCourse != null)
            {
                output.Add(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var command = new GetCoursesByIdsCommand(this.services["Course"] as ICourseService, client.Info.CompletedCourseIds);
            command.Execute();
            var response = command.Response;

            var completedCourses = response.Courses.ToArray();
            PrintCourses(completedCourses);
        }

        #endregion
    }
}
