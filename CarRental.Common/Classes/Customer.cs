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
        public int Id { get; init; }
        public string PersNumber { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public Customer(int id, string persNr, string name, string lastName)
        {
            Id = id;
            PersNumber = persNr;
            FirstName = name;
            LastName = lastName;
        }        
    }
}
