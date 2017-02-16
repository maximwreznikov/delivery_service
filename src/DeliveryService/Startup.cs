using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Core.Bootstrapper;
using DeliveryService.Data;
using DeliveryService.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Nancy.Extensions;
using Nancy.ViewEngines;
using StructureMap;

namespace DeliveryService
{
    public class Startup
    {
        // property for holding configuration
        public IConfigurationRoot Configuration { get; set; }
        private DeliveryBootstrapper _bootstrapper;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            // save the configuration in Configuration property
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<DeliveryServiceSqlLiteContext>();

            var container = new Container();
            container.Configure(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.For<IDeliveryRepository>().Use<DeliverySqlLiteRepository>().ContainerScoped();
                config.For<IUserRepository>().Use<UserSqlLiteRepository>().ContainerScoped();
                config.For<IDateTime>().Add<MachineClockDateTime>().Singleton();

                config.Populate(services);
            });

            _bootstrapper = new DeliveryBootstrapper(container);

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            DeliveryServiceSqlLiteContext context, IContainer container)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            
            app.UseOwin(x => x.UseNancy(o => o.Bootstrapper = _bootstrapper));
//            app.UseWebSockets();
//            app.UseSignalR();
            DbInitializer.Initialize(context);
        }
    }
}
