using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using Nancy.TinyIoc;

namespace DeliveryService
{
    public class DeliveryBootstrapper : DefaultNancyBootstrapper
    {
        public DeliveryBootstrapper()
        {

        }

/*        protected override TinyIoCContainer GetApplicationContainer()
        {
            return _container;
        }*/
    }
}
