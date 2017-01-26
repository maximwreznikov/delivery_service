using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;

namespace DeliveryService.Repositories
{
    public interface IUserRepository
    {
        Person GetPerson(int id);
        bool AddPerson(Person p);
    }
}
