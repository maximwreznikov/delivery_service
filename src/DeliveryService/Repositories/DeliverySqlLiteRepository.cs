using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;

namespace DeliveryService.Repositories
{
    public class DeliverySqlLiteRepository : IDeliveryRepository
    {
        private readonly DeliveryServiceContext _context;

        public DeliverySqlLiteRepository(DeliveryServiceSqlLiteContext context)
        {
            _context = context;
        }

        public IEnumerable<DeliveryObject> AllDeliveries()
        {
            return _context.Deliveries;
        }

        public bool Add(DeliveryObject delivery)
        {
            _context.Deliveries.Add(delivery);
            _context.SaveChanges();
            return true;
        }
    }
}
