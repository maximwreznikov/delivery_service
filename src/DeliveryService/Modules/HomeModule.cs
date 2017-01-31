using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Core;
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
        private readonly IDateTime _systemClock;

        private readonly IDeliveryRepository _repository;
        private readonly IUserRepository _userRepository;

        public HomeModule(IDateTime systemClock,
            IDeliveryRepository repository, 
            IUserRepository userRepository)
        {
            _systemClock = systemClock;
            _repository = repository;
            _userRepository = userRepository;

            Get("/", args => "Hello from Delivery Service running on CoreCLR");

            Get("/ping_clock/{name}", args => Response.AsJson(new {person = new Person {Name = args.name}, clock = _systemClock.Now})
            .WithContentType("application/json")
            .WithStatusCode(HttpStatusCode.OK));
            Get("/GetAvailableDeliveries", args => Response.AsJson(repository.AllDeliveries().ToList())
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK));

            Post("/CreateDelivery/{title}", args => CreateDelivery(args));
            Post("/TakeDelivery/{user:int}.{delivery:int}", args => TakeDelivery(args));
        }

        public Response CreateDelivery(dynamic args)
        {
            var time = _systemClock.Now;
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
            var user = _userRepository.GetPerson(args.user);
            if (user == null) return new NotFoundResponse().WithStatusCode(HttpStatusCode.Unauthorized);

            var myDelivery = _repository.GetDelivery(args.delivery);
            if (myDelivery == null) return new NotFoundResponse().WithStatusCode(HttpStatusCode.NotFound);

            myDelivery.ModificationTime = _systemClock.Now;
            myDelivery.Status = DeliveryStatus.Taken;
            myDelivery.PersonId = user.PersonId;

            DeliveryObject newDelivery = _repository.UpdateDelivery(myDelivery);

            return Response.AsJson(newDelivery)
            .WithContentType("application/json")
            .WithStatusCode(HttpStatusCode.Accepted);
        }
    }
}
