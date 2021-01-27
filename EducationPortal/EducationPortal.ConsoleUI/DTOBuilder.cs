﻿using EducationPortal.BLL.DTO;

namespace EducationPortal.ConsoleUI
{
    public class DTOBuilder
    {
        private static DTOBuilder instance = new DTOBuilder();

        private DTOBuilder()
        {
        }

        public static DTOBuilder GetInstance()
        {
            return instance;
        }

        public UserDTO GetUser(string[] parts)
        {
            return new UserDTO() { Name = parts[0] };
        }

        public AccountDTO GetAccount(string[] parts, bool isReg = false)
        {
            return (isReg) ? new AccountDTO() { Email = parts[0], Login = parts[1], Password = parts[2] }
                           : new AccountDTO() { Email = parts[0], Login = parts[0], Password = parts[1] };
        }
    }
}
