using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Classes
{
    public class Booking : IBooking
    {
        // Use init to set value in constructo ONLY!!
        public int Id { get; init; }
        public IVehicle Vehicle { get; set; }
        public string CustomerName { get; init; }
        public int OdometerRented { get; init; }
        public DateTime Rented { get; init; }
        public int OdometerReturned { get; set; }        
        public DateTime Returned { get; set; }
        public string GetRegNumber() => Vehicle.RegNumber;
        public string GetVehicleStatues() => Vehicle.Statues.ToString();
        public double TotalCost()
        {
            return 0.0;
        }
    }
}
