using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data.Repositories;
using DeliveryService.Models;
using Quartz;

namespace DeliveryService.Scheduling
{
    public class ExpireDeliveriesJob : IJob
    { 
        private readonly IDeliveryRepository _repository;

        public ExpireDeliveriesJob(IDeliveryRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _repository.RemoveAll(d => (d.CreationTime + d.Lifetime) < DateTime.Now || d.Status == DeliveryStatus.Expired);
        }
    }
}
