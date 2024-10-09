using System.ComponentModel.DataAnnotations.Schema;

namespace StorageRoom.Models.Entity
{
   [Table("Passengers")]
    public class Passenger : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
       
        public Guid FlightId { get; set; }
        public Flight Flight { get; set; } 
        
        public List<Baggage> Baggages { get; set; }
    }
}
