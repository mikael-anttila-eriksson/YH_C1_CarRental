using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes
{
    /// <summary>
    /// Stores Booking-information. A booking starts as Open, 
    /// when the item is returned the Booking becomes closed.
    /// </summary>
    public class Booking : IBooking
    {
        public Booking(int id, int vehicleId, int customerId, DateTime rented, int odometerRented)
        {
            BookingIsClosed = false;
            Id = id;
            VehicleId = vehicleId;
            CustomerId = customerId;
            Rented = rented;
            OdometerRented = odometerRented;
            Returned = DateTime.MinValue;
        }
        public bool BookingIsClosed { get; set; }

        #region When initiating a Booking
        //--------------------------------------------------------------- 
        public int Id { get; init; }
        public int VehicleId { get; set; }
        public int CustomerId { get; init; }
        public DateTime Rented { get; init; }
        public int OdometerRented { get; init; }
        //---------------------------------------------------------------
        #endregion When initiating a Booking
        
        #region When Closing a Booking
        //---------------------------------------------------------------
        public DateTime Returned { get; set; }        
        public int OdometerReturned { get; set; }
        public double TotalCost { get; set; }

        //---------------------------------------------------------------
        #endregion When Closing a Booking

        public string ReturnDateToString()
        {
            return Returned == DateTime.MinValue ? "" : Returned.ToString("dd-MM-yyyy");
        }
        public string RentDateToString()
        {
            return Rented.ToString("dd-MM-yyyy");
        }
    }
}
