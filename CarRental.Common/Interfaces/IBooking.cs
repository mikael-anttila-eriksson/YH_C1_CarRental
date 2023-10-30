using CarRental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Interfaces
{
    public interface IBooking
    {
        public int Id { get; init; }
        //public string RegNumber { get; init; }
        public string CustomerName { get; init; }
        public int OdometerRented { get; init; }
        public DateTime Rented { get; init; }
        public int OdometerReturned { get; set; }        
        public DateTime Returned { get; set; }
        //public double TotalCost { get; set; }
        //public VehicleStatues Status { get; set; }
        public string GetRegNumber();
        public string GetVehicleStatues();
        public double TotalCost();
    }
}
