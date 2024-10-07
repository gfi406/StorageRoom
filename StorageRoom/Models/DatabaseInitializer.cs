using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageRoom.Models.Entity;
using StorageRoom;

public class DatabaseInitializer
{
    private readonly ApplicationDbContext _context;

    public DatabaseInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        // Применяем миграции, если они есть
        await _context.Database.MigrateAsync();

        // Проверяем, есть ли уже данные
        if (!_context.Passengers.Any() && !_context.Flights.Any() && !_context.Baggages.Any())
        {
            // Добавляем рейсы
            var flight1 = new Flight { FlightNumber = "", DepartureTime = DateTime.UtcNow.AddHours(2), ArrivalTime = DateTime.UtcNow.AddHours(5) };
            //{ flightnumber = "ab123", departuretime = datetime.utcnow.addhours(2), arrivaltime = datetime.utcnow.addhours(5) };


            await _context.Flights.AddRangeAsync(flight1);
            // Добавляем пассажиров
            //var passenger1 = new Passenger { FirstName = "Иван", LastName = "Иванов", Age = 24};


            //await _context.Passengers.AddRangeAsync(passenger1, passenger2);



            //// Добавляем багаж
            //var baggage1 = new Baggage { Weight = 20.5f, Passenger = passenger1 };


            //await _context.Baggages.AddRangeAsync(baggage1, baggage2);

            // Сохраняем изменения
            await _context.SaveChangesAsync();
        }
    }
}
