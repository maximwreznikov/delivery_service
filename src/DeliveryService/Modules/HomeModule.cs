using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy;

namespace DeliveryService.Modules
{
    public class HomeModule : NancyModule
    {
        private IDeliveryRepository _repository;
        public HomeModule(IDeliveryRepository repository)
        {
            _repository = repository;

            Get("/", args => "Hello from Delivery Service running on CoreCLR");

            Get("/ping_name/{name}", args => Response.AsJson(new Person {Name = args.name}));
            Get("/GetAvailableDeliveries", args => Response.AsJson(repository.AllDeliveries().ToList())
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK));

            Post("/CreateDelivery/{title}", args => CreateDelivery(args));
            Post("/TakeDelivery/{user:int}.{delivery:int}", args => TakeDelivery(args));
        }

        public Response CreateDelivery(dynamic args)
        {
            var time = DateTime.Now;
            var newDelivery = new DeliveryObject
            {
                Title = args.title,
                PersonId = -1,
                Status = DeliveryStatus.Available,
                CreationTime = time,
                ModificationTime = time
            };
            _repository.Add(newDelivery);
            return Response.AsJson(newDelivery)
            .WithContentType("application/json")
            .WithStatusCode(HttpStatusCode.Created);
        }

        public Response TakeDelivery(dynamic args)
        {
            var myDelivery = _repository.AttachDelivery(args.delivery, args.user);
//            var myUser = userRepository.GetPerson();

            return Response.AsJson(myDelivery)
            .WithContentType("application/json")
            .WithStatusCode(HttpStatusCode.Accepted);
        }
    }
}
