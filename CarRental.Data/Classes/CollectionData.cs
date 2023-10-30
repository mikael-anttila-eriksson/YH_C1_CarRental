using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Classes
{
    public class CollectionData : IData
    {
        // readonly
        // - När list har skapats kan den inte skapas igen 
        // - Bara läsa ifrån den
        // - Vi kan fortfarande lägga till saker i listan
        // - Men vi kan inte skapa om listan
        readonly List<IPerson> _persons = new List<IPerson>();
        readonly List<IVehicle> _vehicles = new List<IVehicle>();
        readonly List<IBooking> _bookings = new List<IBooking>();

        public CollectionData() => SeedData();
        void SeedData()
        {

        }

        public IEnumerable<IBooking> GetBookings() => _bookings;

        public IEnumerable<IPerson> GetPersons() => _persons;

        public IEnumerable<IVehicle> GetVehicles(VehicleStatues statues = default) => _vehicles;
    }
}
