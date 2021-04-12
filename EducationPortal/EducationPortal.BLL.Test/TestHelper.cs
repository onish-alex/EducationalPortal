using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.Test
{
    public static class TestHelper
    {      
        public static string GenerateString(int length)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюя1234567890 ";
            var rand = new Random();

            var builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                builder.Append(alphabet[rand.Next(0, alphabet.Length)]);
            }

            return builder.ToString();
        }

    }
}
