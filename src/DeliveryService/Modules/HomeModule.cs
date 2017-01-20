using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Repositories;
using Microsoft.EntityFrameworkCore;
using Nancy;

namespace DeliveryService.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IDeliveryRepository repository)
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");

            Get("/test/{name}", args => Response.AsJson(new Person {Name = args.name}));
            Get("/GetAvailableDeliveries", args =>
            {
                return Response.AsJson(repository.AllDeliveries().ToList())
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK);
            });

            Get("/TakeDelivery/{user}.{delivery}", args => Response.AsJson(new DeliveryObject { Title = args.delivery, PersonId = args.user }));
            Post("/TakeDelivery/{user}.{delivery}", args =>
            {
                var newDelivery = new DeliveryObject
                {
                    Title = args.delivery,
                    PersonId = args.user,
                    Status = DeliveryStatus.Available,
                    CreationTime = DateTime.UtcNow
                };
                repository.Add(newDelivery);
                return Response.AsJson(newDelivery)
                .WithContentType("application/json")
                .WithStatusCode(HttpStatusCode.Created);
            }
            );
        }
    }
}
