﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqTask
{
    public static class CustomerUtilities
    {
        public static Customer GetOldestCustomer()
        {
            return CustomersList.GetCustomers()
                                .OrderBy(a => a.RegistrationDate)
                                .FirstOrDefault();
        }

        public static double GetAverageBalance()
        {
            return CustomersList.GetCustomers()
                                .Average(a => a.Balance);
        }

        public static IEnumerable<Customer> FilterByDate(DateTime from, DateTime to)
        {
            return CustomersList.GetCustomers()
                                .Where(a => a.RegistrationDate >= from
                                         && a.RegistrationDate <= to);
        }

        public static IEnumerable<Customer> FilterById(long from, long to)
        {
            return CustomersList.GetCustomers()
                                .Where(a => a.Id >= from
                                         && a.Id <= to);
        }

        public static IEnumerable<Customer> FilterByNamePart(string namePart)
        {
            return CustomersList.GetCustomers()
                                .Where(a => a.Name.ToLower().Contains(namePart.ToLower()));
        }

        public static void PrintOneMonthRegistered()
        {
            var sortedByMonthAndName = CustomersList.GetCustomers()
                                              .OrderBy(a => a.RegistrationDate.Month)
                                              .ThenBy(a => a.Name)
                                              .GroupBy(a => a.RegistrationDate.Month)
                                              .SelectMany(g => g.Select(a => a));

            foreach (var item in sortedByMonthAndName)
            {
                Console.WriteLine("{0}, {1}", item.Name, item.RegistrationDate.ToString("dd/MM/yy"));
            }
        }

        public static IEnumerable<Customer> SortByField(string fieldName, bool isAscending)
        {
            var propertyInfo = typeof(Customer).GetProperty(fieldName);

            if (isAscending)
                return CustomersList.GetCustomers()
                                    .OrderBy(a => propertyInfo.GetValue(a));

            return CustomersList.GetCustomers()
                                .OrderByDescending(a => propertyInfo.GetValue(a));
        }

        public static void PrintAllCustomers()
        {
            CustomersList.GetCustomers()
                            .Select(a => { Console.Write("{0}, ", a.Name); return a; })
                            .ToArray();
        }
    }
}
