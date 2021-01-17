using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL
{
    public static class DataSettings
    {
        public const string allowableSymbols = "abcdefghijklmnopqrstuvwxyzабвгдеёжхийклмнопрстуфхцчшщъыьэюя1234567890_";

        public const int UserLoginMinCharacterCount = 3;
        public const int UserLoginMaxCharacterCount = 20;

        public const int UserNameMinCharacterCount = 3;
        public const int UserNameMaxCharacterCount = 20;

        public const int UserPasswordMinCharacterCount = 3;
        public const int UserPasswordMaxCharacterCount = 20;

    }
}
