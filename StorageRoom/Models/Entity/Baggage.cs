using StorageRoom.Messages;
using StorageRoom.Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace StorageRoom.Models.Entity
{

    [Table("Baggage")]
    public class Baggage 
    {
        public Guid Id { get; private set; }
        public string? BaggageTag { get; set; } 
        public double Weight { get; set; }     
        
        public Guid PassengerId { get; set; } 
        public Passenger? Passenger { get; set; }
        
        public Baggage()
        {
            Id = Guid.NewGuid();
        }    

        public BaggageMessages ToBaggageMessage()
        {
            return new BaggageMessages
            {
                
                Weight = this.Weight,
                BaggageTag = this.BaggageTag,
                BaggageId = this.Id

                
            };
        }
    }
}