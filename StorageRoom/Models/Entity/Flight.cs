using StorageRoom.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageRoom.Models.Entity
{
    [Table("Flight")]
    public class Flight : BaseEntity
    {
        public string? FlightNumber { get; set; } // Номер рейса
        public string? Destination { get; set; }  // Пункт назначения
        public DateTime DepartureTime { get; set; } // Время отправления
        public DateTime ArrivalTime { get; set; }   // Время прибытия

        // Связь с пассажирами (один ко многим)
        public List<Passenger> Passengers { get; set; }
    }
}