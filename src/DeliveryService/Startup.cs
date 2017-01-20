using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;

namespace DeliveryService
{
    public class Startup
    {
        // property for holding configuration
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            // save the configuration in Configuration property
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
//            var connection = @"server=localhost;user id=test;password=test;database=delivery_service";
//            services.AddDbContext<DeliveryServiceSqlLiteContext>(options => options.UseNpgsql(connection));
            services.AddDbContext<DeliveryServiceSqlLiteContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DeliveryServiceSqlLiteContext context)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOwin(x => x.UseNancy(options => options.Bootstrapper = new DeliveryBootstrapper()));
            DbInitializer.Initialize(context);
        }
    }
}
