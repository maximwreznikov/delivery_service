using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Core.Responses;
using DeliveryService.Data;
using DeliveryService.Data.Repositories;
using DeliveryService.Models;
using Nancy;
using Nancy.Routing;

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

            Get("/", args => "Hello from Delivery Service Nancy running on CoreCLR");

            Get("/ping_clock/{name}", 
                args => Response.AsJson(new {person = new Person {Name = args.name}, clock = _systemClock.Now})
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK));

            
            Get("/Error", args => Response.AsJson("Internal server errror").WithStatusCode(HttpStatusCode.InternalServerError));
        }
    }
}
