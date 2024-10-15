using StorageRoom.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace StorageRoom.Models.Entity
{

    [Table("Baggage")]
    public class Baggage : BaseEntity
    {
        public string? BaggageTag { get; set; } 
        public double Weight { get; set; }     
        
        public Guid PassengerId { get; set; } 
        public Passenger Passenger { get; set; } 
    }
}