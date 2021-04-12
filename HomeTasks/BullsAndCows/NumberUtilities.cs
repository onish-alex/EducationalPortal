using System;

namespace BullsAndCows
{
    public static class NumberUtilities
    {
        public static int[] DivideOnDigits(int number, int numbersCount) 
        {
            var digits = new int[numbersCount];

            int divider = (int)Math.Pow(10, numbersCount - 1);
            int modDivider = 10;

            for (int i = 0; i < numbersCount; i++)
            {
                digits[i] = (number / divider) % modDivider;
                divider /= 10;
            }
            return digits;
        }
    }
}
