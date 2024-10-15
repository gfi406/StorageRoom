using StorageRoom.Models.Entity;

namespace StorageRoom.Models.Dtos
{
    public class BaggageDto
    {
        public Guid Id { get; set; }
        public double Weight { get; set; }
        public string BaggageTag { get; set; }

        public Passenger Passenger { get; set; }

        public List<LinkDto> Links { get; set; } = new List<LinkDto>(); 

    }
}
