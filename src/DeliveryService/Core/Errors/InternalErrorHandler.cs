using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using Nancy.ErrorHandling;

namespace DeliveryService.Core.Errors
{
    public class InternalErrorHandler : IStatusCodeHandler
    {
        private readonly IRootPathProvider _rootPathProvider;

        public InternalErrorHandler(IRootPathProvider rootPathProvider)
        {
            _rootPathProvider = rootPathProvider;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var message = "Server fault description";
            context.Response.Contents = stream =>
            {
                (new StreamWriter(stream) {AutoFlush = true}).Write(message);
            };
        }
    }
}
