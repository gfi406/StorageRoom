using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;

public class PassengerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Baggage> Baggages { get; set; }
    public Guid FlightId { get; set; }
    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
}
