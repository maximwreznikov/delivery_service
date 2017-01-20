using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;
using Nancy;

namespace DeliveryService.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(DeliveryServiceSqlLiteContext context)
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");

            Get("/test/{name}", args => Response.AsJson(new Person {Name = args.name}));
            Get("/GetAvailableDeliveries", args =>
            {
                return Response.AsJson(context.Deliveries.ToList())
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK);
            });

            Get("/TakeDelivery/{user}.{delivery}", args => Response.AsJson(new DeliveryObject { Title = args.delivery, PersonId = args.user }));
            Post("/TakeDelivery/{user}.{delivery}", args =>
            {
                var newDelivery = new DeliveryObject
                {
                    Title = args.delivery,
                    PersonId = args.user
                };
                context.Deliveries.Add(newDelivery);
                return Response.AsJson(newDelivery)
                .WithContentType("application/json")
                .WithStatusCode(HttpStatusCode.Created);
            }
            );
        }
    }
}
