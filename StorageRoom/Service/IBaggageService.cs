using StorageRoom.Models.Entity;

namespace StorageRoom.Service
{
    public interface IBaggageService
    {
        Task<List<Baggage>> GetBaggagesAsync();
        Task<Baggage> GetBaggageByIdAsync(Guid id);
        Task<Baggage> AddBaggegeAsync(Baggage baggage);
        Task<Baggage> UpdateBaggageAsync(Baggage baggage);
        Task DeleteBaggageAsync(Guid id);
        
    }
}
