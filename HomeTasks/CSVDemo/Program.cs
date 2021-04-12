using System;
using System.Linq;
using System.Collections.Generic;

using System.Threading;
using System.Globalization;

namespace CSVDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            var personPropsNames = typeof(Person).GetProperties().Select(a => a.Name);
            var inputStr = string.Empty;
            var chosenProps = new HashSet<string>();
            Console.Write("Properties names: | ");
            foreach (var item in personPropsNames)
            {
                Console.Write("{0} | ", item);
            }

            Console.WriteLine("\nInput property names, separated by comma: ");
            var propNames = Console.ReadLine().Trim().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var item in propNames)
            {
                if (personPropsNames.Contains(item))
                    chosenProps.Add(item);
            }
            CSVFileHelper.Save("demo.csv", PersonList.GetListPerson(), chosenProps);
        }
    }
}
