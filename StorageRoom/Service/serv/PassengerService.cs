using Microsoft.EntityFrameworkCore;
using StorageRoom;
using StorageRoom.Models.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PassengerService : IPassengerService
{
    private readonly ApplicationDbContext _context;

    public PassengerService(ApplicationDbContext context)
    {
        _context = context;
    }
    private static List<Passenger> passengers = new List<Passenger>();

    public async Task<List<Passenger>> GetPassengersAsync()
    {
        return await _context.Passengers
        .Include(p => p.Flight)
        .Include(p => p.Baggages)
        .ToListAsync();
       
    }

    public async Task<Passenger> GetPassengerByIdAsync(Guid id)
    {
        return await _context.Passengers
            .Include(p => p.Flight)
            .Include(p => p.Baggages)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Passenger> AddPassengerAsync(Passenger passenger)
    {
        _context.Passengers.Add(passenger);
        await _context.SaveChangesAsync();
        return passenger;
    }

    public async Task<Passenger> UpdatePassengerAsync(Passenger passenger)
    {
        _context.Passengers.Update(passenger);
        await _context.SaveChangesAsync();
        return passenger;
    }

    public async Task DeletePassengerAsync(Guid id)
    {
        var passenger = await _context.Passengers.FindAsync(id);
        if (passenger != null)
        {
            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();
        }
    }
}
