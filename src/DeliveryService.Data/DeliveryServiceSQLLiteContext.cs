using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data
{
    public class DeliveryServiceSqlLiteContext : DeliveryServiceContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./DeliveryServiceData.db");
        }
    }
}
