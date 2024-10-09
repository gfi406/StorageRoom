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
            var random = new Random();

            // Генерация 5 случайных рейсов
            for (int i = 0; i < 5; i++)
            {
                var flight = new Flight
                {
                    FlightNumber = $"AB{random.Next(100, 999)}", // случайный номер рейса
                    DepartureTime = DateTime.UtcNow.AddHours(random.Next(1, 12)),
                    ArrivalTime = DateTime.UtcNow.AddHours(random.Next(13, 24))
                };

                await _context.Flights.AddAsync(flight);

                // Генерация пассажиров для каждого рейса
                for (int j = 0; j < random.Next(2, 6); j++) // от 2 до 5 пассажиров на рейс
                {
                    var passenger = new Passenger
                    {
                        FirstName = $"Passenger{j}",
                        LastName = $"Last{j}",
                        Age = random.Next(18, 65),
                        Flight = flight
                    };

                    await _context.Passengers.AddAsync(passenger);

                    // Генерация багажа для каждого пассажира (не более 2х)
                    var baggageCount = random.Next(0, 3); // 0, 1 или 2 багажа
                    for (int k = 0; k < baggageCount; k++)
                    {
                        var baggage = new Baggage
                        {
                            Weight = (float)random.NextDouble() * 30 + 5, // вес от 5 до 35 кг
                            Passenger = passenger
                        };
                        await _context.Baggages.AddAsync(baggage);
                    }
                }
            }

            // Сохраняем изменения в базу
            await _context.SaveChangesAsync();
        }
    }
}
