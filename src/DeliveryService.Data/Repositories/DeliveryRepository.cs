using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data.Repositories
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

        public DeliveryObject Create(string title, DateTime time, TimeSpan lifetime)
        {
            var newDelivery = new DeliveryObject
            {
                Title = title,
                PersonId = -1,
                Status = DeliveryStatus.Available,
                CreationTime = time,
                Lifetime = lifetime
            };
            _context.Deliveries.Add(newDelivery);
            return newDelivery;
        }

        public int RemoveAll(Func<DeliveryObject, bool> mark)
        {
            var removeObjects = _context.Deliveries.Where(mark);
            var deliveryObjects = removeObjects as DeliveryObject[] ?? removeObjects.ToArray();
            var count = deliveryObjects.Count();
            _context.Deliveries.RemoveRange(deliveryObjects);
            _context.SaveChanges();
            return count;
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