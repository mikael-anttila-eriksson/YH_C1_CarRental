using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Extensions
{
    public static class PersonExtension
    {
        public static string GetName<T>(this T person) where T : IPerson
            => $"{person.FirstName} {person.LastName}";
    }
}
