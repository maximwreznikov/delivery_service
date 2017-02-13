using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Data;
using DeliveryService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Repositories
{
    public class DeliveryServiceNpsqlRepository : DeliveryRepository<DeliveryServiceNpsqlContext>
    {
        public DeliveryServiceNpsqlRepository(DeliveryServiceNpsqlContext context) : base(context)
        { }
    }

    public class DeliverySqlLiteRepository : DeliveryRepository<DeliveryServiceSqlLiteContext>
    {
        public DeliverySqlLiteRepository(DeliveryServiceSqlLiteContext context) : base(context)
        { }
    }


    public class DeliveryRepository<TContext> : IDeliveryRepository
            where TContext : DeliveryServiceContext
    {
        private readonly TContext _context;

        public DeliveryRepository(TContext context)
        {
            _context = context;
        }

        public IEnumerable<DeliveryObject> AllDeliveries()
        {
            return _context.Deliveries;
        }

        public DeliveryObject GetDelivery(int id)
        {
            return _context.Deliveries.FirstOrDefault(o => o.Id == id);
        }

        public bool Add(DeliveryObject delivery)
        {
            _context.Deliveries.Add(delivery);
            _context.SaveChanges();
            return true;
        }

        public async void UpdateDelivery(DeliveryObject delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }
    }

}