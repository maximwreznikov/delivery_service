using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data
{
    public class DeliveryServiceNpsqlContext : DeliveryServiceContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connection = @"server=localhost;user id=test;password=test;database=delivery_service";
            options.UseNpgsql(connection);
        }
    }
}
