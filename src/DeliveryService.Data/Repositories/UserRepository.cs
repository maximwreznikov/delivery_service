using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;

namespace DeliveryService.Data.Repositories
{
    public class UserServiceNpsqlRepository : UserRepository<DeliveryServiceNpsqlContext>
    {
        public UserServiceNpsqlRepository(DeliveryServiceNpsqlContext context) : base(context)
        { }
    }

    public class UserSqlLiteRepository : UserRepository<DeliveryServiceSqlLiteContext>
    {
        public UserSqlLiteRepository(DeliveryServiceSqlLiteContext context) : base(context)
        { }
    }

    public class UserRepository<TContext> : IUserRepository
        where TContext : DeliveryServiceContext
    {
        private readonly TContext _context;

        public UserRepository(TContext context)
        {
            _context = context;
        }

        public Person GetPerson(int id)
        {
           return new Person {Id = id};
        }

        public bool AddPerson(Person person)
        {
            return true;
        }
    }
}
