using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nancy;

namespace DeliveryService.Core.Responses
{
    public static class ResponseAsError
    {
        public static Response AsError(this IResponseFormatter formatter, HttpStatusCode statusCode, string message)
        {
            return new Response
            {
                StatusCode = statusCode,
                ContentType = "text/plain",
                Contents = stream => (new StreamWriter(stream) { AutoFlush = true }).Write(message)
            };
        }
    }
}
