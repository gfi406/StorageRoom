using StorageRoom.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPassengerService
{
    Task<List<Passenger>> GetPassengersAsync();
    Task<Passenger> GetPassengerByIdAsync(Guid id);
    Task<Passenger> AddPassengerAsync(Passenger passenger);
    Task<Passenger> UpdatePassengerAsync(Passenger passenger);
    Task DeletePassengerAsync(Guid id);
}
