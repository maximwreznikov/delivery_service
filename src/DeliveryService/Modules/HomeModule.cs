using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using DeliveryService.Models;

namespace DeliveryService.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");
            Get("/test/{name}", args => Response.AsJson(new Person() { Name = args.name }));
        }
    }
}
