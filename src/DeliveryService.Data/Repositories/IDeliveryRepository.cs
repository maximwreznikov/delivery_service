using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;

namespace DeliveryService.Data.Repositories
{
    public interface IDeliveryRepository
    {
        IEnumerable<DeliveryObject> AllDeliveries();
        bool Add(DeliveryObject delivery);
        DeliveryObject Create(string title, DateTime time, TimeSpan lifetime);
        int RemoveAll(Func<DeliveryObject,bool> mark);
        DeliveryObject GetDelivery(int id);
        void UpdateDelivery(DeliveryObject delivery);
    }
}
