using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data.Repositories;
using Quartz;

namespace DeliveryService.Scheduling
{
    public class CreateDeliveriesJob : IJob
    {
        private readonly IDeliveryRepository _repository;
        private readonly Random _random;

        public CreateDeliveriesJob(IDeliveryRepository repository)
        {
            _repository = repository;
            _random = new Random(42);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _repository.Create("Test delivery", DateTime.Now, TimeSpan.FromDays(1));
        }
        
    }
}
