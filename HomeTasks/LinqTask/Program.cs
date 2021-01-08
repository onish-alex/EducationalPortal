using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var resultA = CustomerUtilities.GetOldestCustomer();
            var resultB = CustomerUtilities.GetAverageBalance();
            var resultC = CustomerUtilities.FilterByDate(new DateTime(2016, 1, 1), new DateTime(2016, 12, 31));
            var resultD = CustomerUtilities.FilterById(7, 14);
            var resultE = CustomerUtilities.FilterByNamePart("li");
            /*F*/ CustomerUtilities.PrintOneMonthRegistered();
            var resultG1 = CustomerUtilities.SortByField("Balance", true);
            var resultG2 = CustomerUtilities.SortByField("Id", false);
            /*H*/ CustomerUtilities.PrintAllCustomers();
        }
    }
}
