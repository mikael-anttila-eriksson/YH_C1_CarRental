using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Data.Interfaces;
using CarRental.Common.Classes;
using CarRental.Common.Interfaces;
using CarRental.Common.Enums;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    //Fields
    private readonly IData _db;
    //---------------------------------------------------------------
    #region Constructor
    public BookingProcessor(IData db) => _db = db;
    #endregion
    //---------------------------------------------------------------
    #region Properties
    #endregion
    //---------------------------------------------------------------
    #region Methods

    // **************************************************************
    #region Business Logic
    public IEnumerable<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();
        return customers;
    }
    public IEnumerable<IBooking> GetBookings()
    {
        List<IBooking> bookings = new List<IBooking>();
        return bookings;
    }
    public IEnumerable<IVehicle> GetVehicles(VehicleStatues status = default)
    {
        List<IVehicle> vehicles = new List<IVehicle>();
        return vehicles;
    }
    #endregion Business Logic
    // **************************************************************
    #region SubRegion
    #endregion SubRegion
    // **************************************************************
    #endregion Methods
    //---------------------------------------------------------------
}
