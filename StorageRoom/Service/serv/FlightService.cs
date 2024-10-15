using Microsoft.EntityFrameworkCore;
using StorageRoom.Models.Entity;

namespace StorageRoom.Service.serv
{
    public class FlightService : IFlightService
    {

        private readonly ApplicationDbContext _context;
        public FlightService(ApplicationDbContext context)
        {
            _context = context;
        }
        private static List<Flight> flights = new List<Flight>();

        public async Task<List<Flight>> GetFlightsAsync()
        {
            return await _context.Flights
                .Include(p => p.Passengers)
                .ToListAsync();
        }
        public async Task<Flight> GetFlightByIdAsync(Guid id)
        {
            return await _context.Flights.Include(p => p.Passengers).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Flight> AddFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }
        public async Task<Flight> UpdateFlightAsync(Flight flight)
        {
            _context.Update(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task DeleteFlightAsync(Guid id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }
    }
}
