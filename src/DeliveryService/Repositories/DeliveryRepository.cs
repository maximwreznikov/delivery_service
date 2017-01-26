using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Repositories
{
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
            return _context.Deliveries.FirstOrDefault(o => o.DeliveryObjectId == id);
        }

        public bool Add(DeliveryObject delivery)
        {
            _context.Deliveries.Add(delivery);
            _context.SaveChanges();
            return true;
        }

        [FromServices]
        public IUserRepository userRepository
        {
            get;
            set;
        }

        public DeliveryObject AttachDelivery(int delivery, int user)
        {
            var myDelivery = GetDelivery(delivery);
            var myUser = userRepository.GetPerson(user);
            myDelivery.ModificationTime = DateTime.Now;
            myDelivery.Status = DeliveryStatus.Taken;
            myDelivery.PersonId = myUser.PersonId;
            _context.SaveChanges();
            return myDelivery;
        }
    }

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
}
