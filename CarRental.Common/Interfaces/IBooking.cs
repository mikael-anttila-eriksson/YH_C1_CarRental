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
        public bool BookingIsClosed { get; set; }
        public int Id { get; init; }        
        public int CustomerId { get; init; }
        public int VehicleId { get; set; }
        public DateTime Rented { get; init; }
        public DateTime Returned { get; set; }
        public int OdometerRented { get; init; }
        public int OdometerReturned { get; set; }
        public double TotalCost { get; set; }
        
    }
}
