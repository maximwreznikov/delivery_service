using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Repositories;
using DryIoc;
using Nancy;
using Nancy.ViewEngines;

namespace DeliveryService
{
    public class CompositionRoot
    {
        public CompositionRoot(IContainer r)
        {
            r.Register<IDeliveryRepository, DeliverySqlLiteRepository>(Reuse.InCurrentScope);
            r.Register<IUserRepository, UserSqlLiteRepository>(Reuse.InCurrentScope);
            r.Register<IDateTime, MachineClockDateTime>(Reuse.Singleton);

            r.Register<IViewLocationProvider>(Reuse.Singleton, Made.Of(() => new FileSystemViewLocationProvider(Arg.Of<IRootPathProvider>())));
            r.Register<FileSystemViewLocationProvider>(Reuse.Singleton, Made.Of(() => new FileSystemViewLocationProvider(Arg.Of<IRootPathProvider>())));


            //var assemblies = new[] { typeof(ExportedService).GetAssembly() };
            //r.RegisterExports(assemblies);
        }
    }
}
