using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Repositories;
using DryIoc;

namespace DeliveryService
{
    public class CompositionRoot
    {
        public CompositionRoot(IRegistrator r)
        {
            r.Register<IDeliveryRepository, DeliverySqlLiteRepository>(Reuse.InCurrentScope);
            r.Register<IUserRepository, UserSqlLiteRepository>(Reuse.InCurrentScope);
            r.Register<IDateTime, MachineClockDateTime>(Reuse.Singleton);

            //            var assemblies = new[] { typeof(ExportedService).GetAssembly() };
            //            r.RegisterExports(assemblies);
        }
    }
}
