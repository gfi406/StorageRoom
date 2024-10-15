using StorageRoom.Models.Entity;

namespace StorageRoom.Models.Dtos
{
    public class FlightDto
    {
       public Guid Id {  get; set; }        
       public string FlightNumber { get; set; }
       
        public string? Destination { get; set; }  
        
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<Passenger> Passengers { get; set; }
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();

    }
}
