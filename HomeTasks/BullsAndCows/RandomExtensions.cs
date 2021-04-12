using System;
using System.Collections.Generic;

namespace BullsAndCows
{
    public static class RandomExtensions
    {
        public static int GetNumberWithDifferentDigits(this Random rand, int numberLength) 
        {
            int result = 0;
            var digits = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var chosenDigits = new List<int>();

            for (int i = 0; i < numberLength; i++)
            {
                int digitIndex = rand.Next(0, digits.Count);

                result += digits[digitIndex];
                result *= 10;
                digits.RemoveAt(digitIndex);
            }
            result /= 10;
            return result;
        }
    }
}
