using StorageRoom.Models.Entity;
using StorageRoom.Messages;

namespace StorageRoom.RabbitMQ
{
    public static class RabbitMQExtensions
    {
        public static BaggageMessage ToBaggageMessage(this Baggage baggage)
        {
            var message = new BaggageMessage
            {
                BaggageTag = baggage.BaggageTag,
                Weight = baggage.Weight,
                PassengerId = baggage.PassengerId,
                PassengerName = $"{baggage.Passenger.FirstName} {baggage.Passenger.LastName}"
            };
            return message;
        }

        public static PassengerMessage ToPassengerMessage(this Passenger passenger)
        {
            var message = new PassengerMessage
            {
                PassengerId = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Age = passenger.Age,
                FlightId = passenger.FlightId,
                FlightNumber = passenger.Flight.FlightNumber,
                Destination = passenger.Flight.Destination,
                Baggages = passenger.Baggages.Select(b => b.ToBaggageMessage()).ToList()
            };
            return message;
        }

        public static FlightMessage ToFlightMessage(this Flight flight)
        {
            var message = new FlightMessage
            {
                FlightId = flight.Id,
                FlightNumber = flight.FlightNumber,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Passengers = flight.Passengers.Select(p => p.ToPassengerMessage()).ToList()
            };
            return message;
        }
    }
}
