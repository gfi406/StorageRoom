using StorageRoom.Models.Entity;

namespace StorageRoom.Service
{
    public interface IFlightService
    {
        Task<List<Flight>> GetFlightsAsync();
        Task<Flight> GetFlightByIdAsync(Guid id);
        Task<Flight> UpdateFlightAsync(Flight flight);
        Task<Flight> AddFlightAsync(Flight flight);
        Task DeleteFlightAsync(Guid id);


    }
}
