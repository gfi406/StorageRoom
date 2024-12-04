using StorageRoom.Models.Entity;
using StorageRoom.Messages;

namespace StorageRoom.RabbitMQ
{
    public static class RabbitMQExtensions
    {
        public static BaggageMessages ToBaggageMessage(this Baggage baggage)
        {
            var message = new BaggageMessages()
            {
                BaggageTag = baggage.BaggageTag,
                Weight = baggage.Weight,
                BaggageId = baggage.Id,
                PassengerName = $"{baggage.Passenger.FirstName} {baggage.Passenger.LastName}"

            };
            return message;
        }
        //PassengerId = baggage.PassengerId,
        // PassengerName = $"{baggage.Passenger.FirstName} {baggage.Passenger.LastName}"

        public static PassengerMessages ToPassengerMessage(this Passenger passenger)
        {
            var message = new PassengerMessages
            {
                PassengerId = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Age = passenger.Age,
               // FlightId = passenger.FlightId,
                //FlightNumber = passenger.Flight.FlightNumber,
               // Destination = passenger.Flight.Destination,
               // Baggages = passenger.Baggages.Select(b => b.ToBaggageMessage()).ToList()
            };
            return message;
        }

        public static FlightMessages ToFlightMessage(this Flight flight)
        {
            var message = new FlightMessages
            {
                FlightId = flight.Id,
                FlightNumber = flight.FlightNumber,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,


            };
            return message;
        }
    }
}
