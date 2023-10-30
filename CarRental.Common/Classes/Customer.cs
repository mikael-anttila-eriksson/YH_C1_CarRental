using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Classes
{
    public class Customer : IPerson
    {
        public int PersNumber { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public Customer(int persNr, string name, string lastName)
        {
            PersNumber = persNr;
            FirstName = name;
            LastName = lastName;
        }
    }
}
