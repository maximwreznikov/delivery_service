using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;

namespace DeliveryService.Modules
{
    public class EventsModule : NancyModule
    {
        public EventsModule():base("events")
        {
            Get("/", args => View["TestEvents"]);
        }
    }
}
