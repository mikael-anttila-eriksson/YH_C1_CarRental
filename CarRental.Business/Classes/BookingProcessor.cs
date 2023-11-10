using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Data.Interfaces;
using CarRental.Common.Classes;
using CarRental.Common.Interfaces;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using System.Transactions;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    //Fields
    private readonly IData _db;
    /// <summary>
    /// A string to store genereated error messages from methods in this class.
    /// </summary>
    private string _errorMsg = string.Empty;
    //---------------------------------------------------------------
    #region Constructor
    /// <summary>
    /// Runs via injection at runtime.
    /// </summary>
    /// <param name="db"></param>
    public BookingProcessor(IData db) => _db = db;
    #endregion
    //---------------------------------------------------------------
    #region Error
    public string ErrorMessage => _errorMsg;
    private void ResetErrorMessage() => _errorMsg = "";
        #endregion  Error
    //---------------------------------------------------------------
    #region Methods

    // **************************************************************
    #region Getters
    
    //---------------------------------------------------------------
    public IEnumerable<T> GeneralGetAll<T>() where T : class
    {
        ResetErrorMessage();
        return _db.GenericGet<T>(null, out _errorMsg);
    }
    public T? GeneralGetSingle<T>(Func<T, bool> lamba) where T : class
    {
        ResetErrorMessage();
        return _db.GenericSingle<T>(lamba);
    }
    #endregion Getters
    // **************************************************************
    #region Rent and Return
    public async Task RentVehicle(int customerId, int vehicleId)
    {
        ResetErrorMessage();

        // Make a mock-process -> Delay 8 seconds
        await Task.Delay(8000);

        // Chek input
        try
        {
            // Does input ID.s exist?
            if (GeneralGetSingle<IPerson>(c => c.Id == customerId) == null)
            {
                _errorMsg = "Customer ID does not exist!";
                return;
            }
            if (GeneralGetSingle<IVehicle>(c => c.VehicleId == vehicleId) == null)
            {
                _errorMsg = "Vehicle ID does not exist!";
                return;
            }
        }
        catch (Exception ex)
        {
            // Other error
            _errorMsg = ex.Message;
            return;
        }        

        // Make new Booking 
        // Vehicle info: Id, Reg, Odo
        IVehicle vehicle = GeneralGetSingle<IVehicle>(c => c.VehicleId == vehicleId);
        // Change Vehicle Status
        vehicle.Status = VehicleStatus.Booked;
        // Add booking
        IBooking booking = new Booking(_db.NextBookingId, vehicleId, customerId, MakeRandomDate(), vehicle.Odometer);
        _db.GenericAdd(booking, out _errorMsg);
        return;         
    }
    public void ReturnVehicle(int vehicleId, int? distance)                     
    {
        // Is there a Booking for this vehicle?
        string gg = "jer";
        IBooking booking = null;
        try
        {
            booking = GeneralGetSingle<IBooking>(b => b.VehicleId == vehicleId && b.BookingIsClosed == false);
            if (booking is null)
            {
                _errorMsg = "Booking does not exist.";
                return;
            }
        }
        catch (Exception ex)
        {
            _errorMsg = ex.Message;
            
        }
        try
        {
            // close booking
            // Get Vehicle
            IVehicle vehicle = GeneralGetSingle<IVehicle>(p => p.VehicleId == booking.VehicleId);

            // Change vehicle parameters             
            vehicle.Odometer += distance is null ? 0 : (int)distance;
            vehicle.Status = VehicleStatus.Available;

            // Change Booking paramters
            booking.BookingIsClosed = true;
            booking.Returned = DateTime.Now;
            booking.OdometerReturned = vehicle.Odometer;
            booking.TotalCost = TotalCostRenting(booking, vehicle);
            // update Bookings list
            _db.UpdateBooking(booking);
            
        }
        catch (Exception ex)
        {
            _errorMsg = ex.Message;
            
        }
        
    }
    #endregion Rent and Return
    // **************************************************************
    #region Add
    public void AddVehicle(string make, string regNum, int? odometer,    
        double? costKm, double? costDay, string type)
    {
        ResetErrorMessage();

        // Check input
        bool typeOk = CheckVehicleType(type, out VehicleTypes theType);
        bool DataOk = CheckVehicleInput(odometer, costKm, costDay);
        bool regOk = CheckRegNumber(regNum);
        bool makeOk = CheckManufacturer(make, out Manufacturer manufacturer);        

        // Add vehicle
        IVehicle newVehicle = null;
        // Input Correct?
        if(DataOk && regOk && makeOk && typeOk)
        {
            // Is VehivleType a Car? Else a motorcycle
            
            if (theType == VehicleTypes.Motorcycle)
            {
                // Add motorcycle
                newVehicle = new Motorcycle(_db.NextVehicleId, regNum, odometer ?? default(int), costKm ?? default(double), costDay ?? default(double), manufacturer);

            }
            else
            {
                // Add car
                newVehicle = new Car(_db.NextVehicleId, regNum, theType, odometer ?? default, costKm ?? default, costDay ?? default, manufacturer);
            }
            // Add to "Database"
            bool add = _db.GenericAdd(newVehicle, out _errorMsg);            
        }
        else
        {
            _errorMsg = "Input was not correct";
        }        
    }
    public void AddIPerson(string persNr, string name, string lastName)
    {
        // check input
        bool numberOk = CheckPersonNumber(persNr);
        bool nameOk = (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(lastName));
        if(numberOk && numberOk)
        {
            // Add person
            IPerson newPerson = new Customer(_db.NextPersonId, persNr, name, lastName);
            _db.GenericAdd(newPerson, out _errorMsg);
        }
        else
        {
            _errorMsg = "Person detail was not correct";
        }
    }
    #endregion Add
    // **************************************************************
    #region Get Enums
    public string[] VehicleStatusNames => _db.VehicleStatusNames();
    public string[] VehicleTypeNames => _db.VehicleTypeNames();
    public string[] ManufacturerNames => _db.ManufaturerNames();
    #endregion Get Enums
    // **************************************************************
    #region Checks
    private bool CheckPersonNumber(string persNr)
    {
        if (string.IsNullOrEmpty(persNr)) return false;
        if (persNr.Length != 12) return false;
        if (!persNr.All(Char.IsDigit)) return false;
        return true;
    }
    /// <summary>
    /// Check that the number inputs are not null and above zero.
    /// </summary>
    /// <param name="odometer"></param>
    /// <param name="costKm"></param>
    /// <param name="costDay"></param>
    /// <returns>
    /// true if input OK.
    /// </returns>
    private bool CheckVehicleInput(int? odometer, double? costKm, double? costDay)
    {
        bool ok = false;
        // Does the parameters have a value?
        ok = odometer.HasValue && costKm.HasValue && costDay.HasValue;
        if (!ok) return false;
        // Are values above zero?
        ok = odometer > 0 && costKm > 0 && costDay > 0;
        return ok;
    }
    /// <summary>
    /// Check that the Registration number is not null or empty,
    /// is 6 characters long, starts with three letters and
    /// ends with three numbers.
    /// </summary>
    /// <param name="regNr"></param>
    /// <returns></returns>
    private bool CheckRegNumber(string regNr)
    {
        if (string.IsNullOrEmpty(regNr)) return false;
        if(regNr.Length != 6) return false;
        if(!regNr.Substring(0, 3).All(Char.IsLetter)) return false;
        if (!regNr.Substring(3, 3).All(Char.IsDigit)) return false;
        
        return true;
    }
    private bool CheckManufacturer(string make, out Manufacturer manufacturer)
    {
        return Enum.TryParse<Manufacturer>(make, out manufacturer);
    }
    private bool CheckVehicleType(string inType, out VehicleTypes outType)
    {
        return Enum.TryParse<VehicleTypes>(inType, out outType);
    }
    #endregion Checks
    // **************************************************************
    #region Other
    private double TotalCostRenting(IBooking booking, IVehicle vehicle)
    {
        int usedKm = booking.OdometerReturned - booking.OdometerRented;
        double usedDays = (booking.Returned - booking.Rented).TotalDays;

        double cost = (vehicle.CostPerKm * usedKm) + (vehicle.CostPerDay * usedDays); 

        return Math.Round(cost,2);
    }
    /// <summary>
    /// Make a random day from january to october - 2023.
    /// </summary>
    /// <returns></returns>
    private DateTime MakeRandomDate()
    {
        Random random = new Random();
        int month = random.Next(1, 11);
        int day = random.Next(1, 31);
        return new DateTime(2023, month, day);
    }
    public string GetRegNumber(int id, out string errorMsg)
    {
        string regNumber = null;
        errorMsg = "";
        try
        {
            regNumber = GeneralGetSingle<IVehicle>(p => p.VehicleId == id).RegNumber;
        }
        catch 
        {
            errorMsg = "Info: Could not load registration numbers in booking table.";
        }
        return regNumber;
    }
    public string GetFullName(int id, out string errorMsg)
    {
        string name = null;
        errorMsg = "";
        try
        {
            name = GeneralGetSingle<IPerson>(p => p.Id == id).GetName();

        }
        catch 
        {            
            errorMsg = "Info: Could not load names in booking table.";
        }

        return name;
    }
    #endregion Other
    // **************************************************************
    #endregion Methods
    //---------------------------------------------------------------
}
