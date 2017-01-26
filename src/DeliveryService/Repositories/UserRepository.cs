using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data;
using DeliveryService.Models;

namespace DeliveryService.Repositories
{
    public class UserRepository<TContext> : IUserRepository
        where TContext : DeliveryServiceContext
    {
        private readonly TContext _context;

        public UserRepository(TContext context)
        {
            _context = context;
        }

        public Person GetPerson(int missing_name)
        {
            throw new NotImplementedException();
        }

        public bool AddPerson(Person missing_name)
        {
            throw new NotImplementedException();
        }
    }
}
