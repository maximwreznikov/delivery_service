using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Models;

namespace DeliveryService.Repositories
{
    public interface IDeliveryRepository
    {
        IEnumerable<DeliveryObject> AllDeliveries();
        bool Add(DeliveryObject delivery);
        DeliveryObject GetDelivery(int id);
        void UpdateDelivery(DeliveryObject delivery);
    }
}
