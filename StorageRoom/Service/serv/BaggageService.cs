using Microsoft.EntityFrameworkCore;
using StorageRoom.Models.Entity;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace StorageRoom.Service.serv
{
    public class BaggageService : IBaggageService
    {
        private readonly ApplicationDbContext _context;
     
        private readonly RabbitMqChannelFactory _channelFactory;        
        public BaggageService(ApplicationDbContext context, RabbitMqChannelFactory channelFactory)
        {
            _context = context;
            
            _channelFactory = channelFactory;   
        }

        private static List<Baggage> baggages = new List<Baggage>();
        public async Task<List<Baggage>> GetBaggagesAsync()
        {
            return await _context.Baggages
            .Include(p => p.Passenger)
            .ToListAsync();
        }
        public async Task<Baggage> GetBaggageByIdAsync(Guid id)
        {
            return await _context.Baggages
                .Include(p => p.Passenger)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Baggage> AddBaggegeAsync(Baggage baggage)
        {
            _context.Baggages.Add(baggage);
            await _context.SaveChangesAsync();
            return baggage;
        }
        public async Task<Baggage> UpdateBaggageAsync(Baggage baggage)
        {
            _context.Baggages.Update(baggage);
            await _context.SaveChangesAsync();
           
            return baggage;
        }
        public async Task DeleteBaggageAsync(Guid id)
        {
            var baggage = await _context.Baggages.FindAsync(id);
            if (baggage != null)
            {
                _context.Baggages.Remove(baggage);
                await _context.SaveChangesAsync();
            }


        }
        //private void SendBaggageToQueue(Baggage baggage)
        //{
        //    string message = $"Baggage Updated: {baggage.BaggageTag}, Weight: {baggage.Weight}, PassengerId: {baggage.PassengerId}";
        //    var body = Encoding.UTF8.GetBytes(message);

        //    _rabbitChannel.BasicPublish(
        //        exchange: "",
        //        routingKey: "baggage_queue",  // Используйте вашу очередь
        //        basicProperties: null,
        //        body: body);

        //    Console.WriteLine(" [x] Sent {0}", message);
        //}


    }
}
