using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Interfaces
{
    public interface IPerson
    {
        public int PersNumber { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

    }
}
