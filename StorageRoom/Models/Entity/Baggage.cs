using StorageRoom.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace StorageRoom.Models.Entity
{

   // [Table("Baggage")]
    public class Baggage : BaseEntity
    {
        public string? BaggageTag { get; set; } // Тег багажа
        public double Weight { get; set; }     // Вес багажа

        // Измените тип PassengerId на Guid
        public Guid PassengerId { get; set; } // Внешний ключ на пассажира
        public Passenger Passenger { get; set; } // Связь с пассажиром
    }
}