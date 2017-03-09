using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Core.Responses;
using DeliveryService.Data.Repositories;
using DeliveryService.Models;
using Nancy;

namespace DeliveryService.Modules
{
    public class DeliveryModule : NancyModule
    {
        private readonly IDateTime _systemClock;

        private readonly IDeliveryRepository _repository;
        private readonly IUserRepository _userRepository;

        public DeliveryModule(IDateTime systemClock,
            IDeliveryRepository repository,
            IUserRepository userRepository): base("/delivery")
        {
            _systemClock = systemClock;
            _repository = repository;
            _userRepository = userRepository;

            Get("/get_available",
                args => Response.AsJson(repository.AllDeliveries().Where(x => x.Status == DeliveryStatus.Available).ToList())
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK));

            Post("/create/{title}", args => CreateDelivery(args));
            Post("/clear_expired}", args => ClearExpired(args));
            Post("/take/{user:int}.{delivery:int}", args => TakeDelivery(args));
        }

        private Response CreateDelivery(dynamic args)
        {
            int? lifetime = args.lifetime;
            DeliveryObject newDelivery = _repository.Create(args.title, _systemClock.Now, lifetime.HasValue ? TimeSpan.FromSeconds(lifetime.Value) : TimeSpan.FromDays(1));
            return Response.AsJson(newDelivery)
            .WithContentType("application/json")
            .WithStatusCode(HttpStatusCode.Created);
        }

        private Response ClearExpired(dynamic args)
        {
            _repository.RemoveAll(d => (d.CreationTime + d.Lifetime) < DateTime.Now);
            return HttpStatusCode.OK;
        }

        private Response TakeDelivery(dynamic args)
        {
            var user = _userRepository.GetPerson(args.user);
            if (user == null) return Response.AsError(HttpStatusCode.Unauthorized, "Can`t find user");

            DeliveryObject myDelivery = _repository.GetDelivery(args.delivery);
            if (myDelivery == null) return Response.AsError(HttpStatusCode.NotFound, "Can`t find delivery");
            if (myDelivery.Status != DeliveryStatus.Available) return Response.AsError(HttpStatusCode.UnprocessableEntity, "Wrong status! Delivery must have status Available");
            if (myDelivery.CreationTime + myDelivery.Lifetime < _systemClock.Now)
            {
                myDelivery.Lifetime = TimeSpan.Zero;
                myDelivery.Status = DeliveryStatus.Expired;
                _repository.UpdateDelivery(myDelivery);
                return Response.AsError(HttpStatusCode.BadRequest, "Error! Delivery expired!");
            }

            myDelivery.Lifetime = TimeSpan.Zero;
            myDelivery.Status = DeliveryStatus.Taken;
            myDelivery.PersonId = user.Id;

            _repository.UpdateDelivery(myDelivery);

            return Response.AsJson(myDelivery)
            .WithContentType("application/json")
            .WithStatusCode(HttpStatusCode.Accepted);
        }
    }
}
