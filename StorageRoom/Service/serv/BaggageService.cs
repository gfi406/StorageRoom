using Microsoft.EntityFrameworkCore;
using StorageRoom.Models.Entity;

namespace StorageRoom.Service.serv
{
    public class BaggageService : IBaggageService
    {
        private readonly ApplicationDbContext _context;
        public BaggageService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static List<Baggage> baggages = new List<Baggage>();
        public async Task<List<Baggage>> GetBaggagesAsync()
        {
            return await _context.Baggages
            .Include(p => p.Passenger)
            .ToListAsync();
        }
        public async Task<Baggage> GetBaggageByIdAsync(Guid id)
        {
            return await _context.Baggages
                .Include(p => p.Passenger)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Baggage> AddBaggegeAsync(Baggage baggage)
        {
            _context.Baggages.Add(baggage);
            await _context.SaveChangesAsync();
            return baggage;
        }
        public async Task<Baggage> UpdateBaggageAsync(Baggage baggage)
        {
            _context.Baggages.Update(baggage);
            await _context.SaveChangesAsync();
            return baggage;
        }
        public async Task DeleteBaggageAsync(Guid id)
        {
            var baggage = await _context.Baggages.FindAsync(id);
            if (baggage != null)
            {
                _context.Baggages.Remove(baggage);
                await _context.SaveChangesAsync();
            }


        }

       
    }
}
