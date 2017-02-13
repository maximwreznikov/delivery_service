using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core.Bootstrapper;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Nancy;
using Nancy.Routing;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace DeliveryService
{
    public class DeliveryBootstrapper : DryIocNancyBootstrapper
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
