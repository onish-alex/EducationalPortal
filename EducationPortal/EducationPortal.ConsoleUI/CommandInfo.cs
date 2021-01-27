using System.Collections.Generic;

namespace EducationPortal.ConsoleUI
{
    public class CommandInfo
    {
        public readonly static Dictionary<string, CommandInfo> Storage = new Dictionary<string, CommandInfo>()
        {
            {"reg", new CommandInfo()
                        {
                          ParamsCount = 4,
                          Description = "reg [email] [login] [password] [username]\nРегистрация нового пользователя\n"
                        }
            },
            {"login", new CommandInfo()
                          {
                            ParamsCount = 2,
                            Description = "login [login | email] [password]\nАвторизация пользователя\n"
                          }
            },
            {"logout", new CommandInfo()
                          {
                            ParamsCount = 0,
                            Description = "logout\nВыход из системы\n"
                          }
            },
            {"help", new CommandInfo() 
                          {
                            ParamsCount = 0,
                            Description = "help\nВывод списка команд\n"
                          } 
            },
            {"exit", new CommandInfo()
                          {
                            ParamsCount = 0,
                            Description = "exit\nВыход из программы\n"
                          }
            }
        };

        public int ParamsCount { get; set; }
        public string Description { get; set; }

        private CommandInfo()
        {

        }
    }
}
