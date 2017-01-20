using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;

namespace DeliveryService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DeliveryServiceContext context)
        {
            context.Database.EnsureCreated();

            // Look for any records.
            if (context.Deliveries.Any() || context.Persons.Any())
            {
                return;   // DB has been seeded
            }

            context.Persons.Add(new Person {Name = "Admin"});

            context.Deliveries.Add(new DeliveryObject{Title = "Big Tea Box", Status = DeliveryStatus.Available, CreationTime = DateTime.UtcNow});

            context.SaveChanges();
        }
    }
}
