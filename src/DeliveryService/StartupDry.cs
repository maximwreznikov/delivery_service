using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Core.Bootstrapper;
using DeliveryService.Data;
using DeliveryService.Repositories;
using DryIoc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Nancy;
using Nancy.Extensions;
using Nancy.TinyIoc;
using DryIoc.MefAttributedModel;
using DryIoc.Microsoft.DependencyInjection;
using Nancy.ViewEngines;

namespace DeliveryService
{
    public class StartupDry
    {
        // property for holding configuration
        public IConfigurationRoot Configuration { get; set; }
        private DeliveryBootstrapper _bootstrapper;

        public StartupDry(IHostingEnvironment env)
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
//            services.AddMvc();

            var container = new Container();
            var provider = container
                .WithMef()
                .WithDependencyInjectionAdapter(services,
                    throwIfUnresolved: type => type.Name.EndsWith("Module"))
                .With(rules => rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()))
                .ConfigureServiceProvider<CompositionRoot>();

//            _bootstrapper = new DeliveryBootstrapper(container);

            return provider;
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

//            var b = container.GetServiceRegistrations().Where(s => s.ServiceType == typeof(IViewLocationProvider)).ToList();
//            var b1 = container.GetServiceRegistrations().Where(s => s.ServiceType == typeof(FileSystemViewLocationProvider)).ToList();
//            var a = container.Resolve<IViewLocationProvider>();

            app.UseOwin(x => x.UseNancy(o => o.Bootstrapper = _bootstrapper));
//            app.UseWebSockets();
//            app.UseSignalR();
            DbInitializer.Initialize(context);
        }
    }
}
