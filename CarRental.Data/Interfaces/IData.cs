using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Interfaces
{
    public interface IData
    {
        int NextVehicleId { get; }
        int NextPersonId { get; }
        int NextBookingId { get; }
        public string[] VehicleStatusNames();
        public string[] VehicleTypeNames();
        public string[] ManufaturerNames();
        IEnumerable<T> GenericGet<T>(Func<T, bool>? expression, out string errorMsg) where T : class;
        IEnumerable<T> GenericGet2<T>(Func<T, bool>? expression, out string errorMsg) where T : class;
        public IEnumerable<T> GenericGet3<T>(Func<T, bool>? expression, out string errorMsg) where T : class;
        public T? GenericSingle<T>(Func<T, bool> expression) where T : class;
        public bool GenericAdd<T>(T item, out string errorMsg) where T : class;
        public bool UpdateBooking(IBooking booking);
    }
}
