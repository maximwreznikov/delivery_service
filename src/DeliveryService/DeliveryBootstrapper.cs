using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core.Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using Nancy;
using Nancy.Routing;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;

namespace DeliveryService
{
    public class DeliveryBootstrapper : StructureMapNancyBootstrapper
    {
        private  readonly IContainer _container;
        public DeliveryBootstrapper(IContainer container)
        {
            _container = container;
        }

        protected override IContainer GetApplicationContainer()
        {
            return _container;
        }
    }
}
