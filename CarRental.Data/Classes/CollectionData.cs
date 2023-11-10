using CarRental.Common.Enums;
using CarRental.Common.Classes;
using CarRental.Data.Interfaces;
using CarRental.Common.Interfaces;
using CarRental.Common.Extensions;
using System.Reflection;
using System.Linq.Expressions;

namespace CarRental.Data.Classes;

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
        _persons.Add(new Customer(NextPersonId, "6205231256", "Michael", "Scott"));
        _persons.Add(new Customer(NextPersonId, "7411042375", "Pam", "Beesly"));
        _persons.Add(new Customer(NextPersonId, "7910224586", "Jim", "Halpert"));
        _persons.Add(new Customer(NextPersonId, "8004109546", "Erin", "Hannon"));
        _persons.Add(new Customer(NextPersonId, "6601205162", "Dwight", "Schrute"));


        _vehicles.Add(new Car(NextVehicleId, "ART482", VehicleTypes.Combi, 8051, 8.1, 420, Manufacturer.Volvo));
        _vehicles.Add(new Car(NextVehicleId, "DJA965", VehicleTypes.Sedan, 41860, 14.7, 290, Manufacturer.Saab));
        _vehicles.Add(new Car(NextVehicleId, "IGP330", VehicleTypes.Van, 27364, 19.5, 370, Manufacturer.Volkswagen));
        _vehicles.Add(new Car(NextVehicleId, "YKV029", VehicleTypes.Sedan, 3403, 0.1, 490, Manufacturer.Tesla));
        _vehicles.Add(new Motorcycle(NextVehicleId, "GOO999", 5980, 5, 370, Manufacturer.Yamaha));
        _vehicles.Add(new Motorcycle(NextVehicleId, "JAP238", 450, 6.6, 550, Manufacturer.Kawasaki));

        // Booking - Rented
        _bookings.Add(new Booking(NextBookingId, _vehicles[0].VehicleId, _persons[2].Id, new DateTime(2023, 10, 4), _vehicles[0].Odometer));
        _vehicles[0].Status = VehicleStatus.Booked;
        // Booking - Returned
        _bookings.Add(new Booking(NextBookingId, _vehicles[3].VehicleId, _persons[1].Id, new DateTime(2023, 10, 8), _vehicles[3].Odometer));
        _vehicles[3].Status = VehicleStatus.Available;
        _vehicles[3].Odometer += 45;
        _bookings[1].BookingIsClosed = true;
        _bookings[1].Returned = DateTime.Now;
        _bookings[1].OdometerReturned = _vehicles[3].Odometer;
        _bookings[1].TotalCost = 220.55;

    }

    // --------- Ska ha EN Add<T> och EN Get<T>, En Single<T>, som funkar för Alla Typer!!! -------------
    // **************************************************************
    #region Generic Add & Get
    // **************************************************************
    #region Generic troubleShoot
    public IEnumerable<T> GenericGet3<T>(Func<T, bool>? expression, out string errorMsg) where T : class // klar testa med customer !!!!
    {
        errorMsg = "";
        try
        {
            Type type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var collection = fields.FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly);
            
            //.FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly)
            //?? throw new InvalidOperationException("Unsupported type, 3");

            var value = collection.GetValue(this) ?? throw new InvalidDataException("Some data ProBlem?!?, 3");

            var query = ((List<T>)value).AsQueryable();

            if (expression == null) return query;

            return query.Where(expression);
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            throw;
        }
          
    }
    public IEnumerable<T> GenericGet2<T>(Func<T, bool>? expression, out string errorMsg) where T : class
    {
        errorMsg = "";
        List<T> list = null;
        try
        {
            Type type = GetType();
            FieldInfo[] fields = type.GetVariables();
            FieldInfo fieldInfo = fields.FindCollection<T>();
            Object? obj = fieldInfo.GetData(this);
            bool ok = obj is not null;
            ok = obj is List<T>;
            IQueryable<T> query; //= obj.ToQueryable<T>();
            if(ok)
            {
                var query2 = ((List<T>)obj).AsQueryable();
                query = (IQueryable<T>)obj;
                list = query.Filter(expression);
            }
            else
            {
                int i = 0;
            }
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            
        }
        

        return list;
    }
    #endregion Generic troubleShoot
    // **************************************************************
    public IEnumerable<T> GenericGet<T>(Func<T, bool>? expression, out string errorMsg) where T : class
    {        
        try
        {
            errorMsg = "";
            return GetType()
                .GetVariables()
                .FindCollection<T>()
                .GetData(this)
                .myAsQueryable<T>()
                .Filter(expression);
        }
        catch(InvalidDataException ex)
        {
            errorMsg = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            errorMsg = ex.Message;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;            
        }
        return new List<T>();
    }
    public T? GenericSingle<T>(Func<T, bool> expression) where T : class
    {
        var targetCollection = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly)
            ?? throw new InvalidOperationException("Unsupported type");

        var value = targetCollection.GetValue(this)
            ?? throw new InvalidDataException("Data error");

        //var query = (IQueryable<T>)value;
        var query = ((List<T>)value).AsQueryable();

        return query.SingleOrDefault(expression);        
    }
    public bool GenericAdd<T>(T item, out string errorMsg) where T : class
    {        
        try
        {
            errorMsg = "";
            var targetCollection = new List<T>();
            targetCollection = (List<T>)GetType()
                .GetVariables()
                .FindCollection<T>()
                .GetData(this);
            // Add to collection
            targetCollection.Add(item);
            
            return true;
        }
        catch (InvalidDataException ex)
        {
            errorMsg = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            errorMsg = ex.Message;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            
        }
        return false;
    }
    #endregion Generic Add & Get
    // **************************************************************
    #region Rent
    public bool UpdateBooking(IBooking booking)
    {
        var oldBook = GenericSingle<IBooking>(b => b.Id == booking.Id);
        if(oldBook != null)
        {
            oldBook = booking;
            return true;
        }
        return false;
    }
    #endregion Rent
    // **************************************************************
    #region Simple Gets - G
    public IEnumerable<IBooking> GetBookings() => _bookings;

    public IEnumerable<IPerson> GetPersons() => _persons;

    public IEnumerable<IVehicle> GetVehicles(VehicleStatus status = default) //=> (IEnumerable<IVehicle>)_vehicles.Select(f => f.Status == status);
    {
        VehicleStatus st = status;
        IEnumerable<IVehicle> list = _vehicles;
        if(status == VehicleStatus.Available || status == VehicleStatus.Booked)         
        {
            // Do selection
            list = list.Where(v => v.Status == status).ToList();    //where behöver ToList() för att funkar!!!
        }
        else
        {
            // Take all
        }
        
        return list;
    }
    #endregion Simple Gets - G
    // **************************************************************
    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(b => b.VehicleId) + 1;
    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(b => b.Id) + 1;
    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(b => b.Id) + 1;

    public string[] VehicleStatusNames()
    {
        return Enum.GetNames(typeof(VehicleStatus));
    }
    public string[] VehicleTypeNames() => Enum.GetNames(typeof(VehicleTypes));
    public string[] ManufaturerNames()
    {
        string[] ert = Enum.GetNames(typeof(Manufacturer));
        return ert;
    }
    // **************************************************************
}
