using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DeliveryService.Data
{
    public class DeliveryServiceContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<DeliveryObject> Deliveries { get; set; }

        public DeliveryServiceContext():base()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<DeliveryObject>().ToTable("Deliveries");

//            modelBuilder.Entity<DeliveryObject>()
//                .HasKey(c => new { c.UserId });
        }
    }
}
