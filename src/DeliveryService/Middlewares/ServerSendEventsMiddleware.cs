using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Middlewares
{
    public static class ServerSendEventsMiddlewareExtensions
    {
        public static IApplicationBuilder UseServerSendsEvents(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }

    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
//                var culture = new CultureInfo(cultureQuery);
//
//                CultureInfo.CurrentCulture = culture;
//                CultureInfo.CurrentUICulture = culture;

            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }

    public class ServerSendEventsMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly string _Url;

        public ServerSendEventsMiddleware(RequestDelegate next, string url)
        {
            _Next = next;
            _Url = url;
        }

        public void Use()
        {
//            async (HttpContext context, RequestDelegate next) =>
//            {
//                if (context.Request.Path.ToString().Equals("/sse"))
//                {
//                    var response = context.Response;
//                    response.Headers.Add("Content-Type", "text/event-stream");
//
//                    for (var i = 0; true; ++i)
//                    {
//                        // WriteAsync requires `using Microsoft.AspNetCore.Http`
//                        await response.WriteAsync($"data: Middleware {i} at {DateTime.Now}\r\r");
//
//                        response.Body.Flush();
//                        await Task.Delay(5 * 1000);
//                    }
//                }
//
//                await next.Invoke();
//            }
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value == _Url)
            {
                Initialize(httpContext);
                // await InitializeAsync(httpContext);
                return;
            }
            await _Next(httpContext);
        }

        private void Initialize(HttpContext httpContext)
        {
            httpContext.Response.Clear();

            httpContext.Response.ContentType = "text/event-stream";
            httpContext.Response.Headers.Add("Cache-Control", "no-cache");
            httpContext.Response.Headers.Add("Connection", "keep-alive");
            httpContext.Response.Headers.Add("Keep-Alive", "timeout=15, max=100");

            var stream = httpContext.Response.Body;

            // block 1
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            {
                writer.Write("data:initialize\n\n");
                writer.Flush();
            }

            // block 2
            Task.Run(() => {
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, true))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        writer.Write("data:test + " + i + " \n\n");
                        writer.Flush();
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        private async Task InitializeAsync(HttpContext httpContext)
        {
            httpContext.Response.Clear();

            httpContext.Response.ContentType = "text/event-stream";
            httpContext.Response.Headers.Add("Cache-Control", "no-cache");
            httpContext.Response.Headers.Add("Connection", "keep-alive");
            httpContext.Response.Headers.Add("Keep-Alive", "timeout=15, max=100");

            var writer = new StreamWriter(httpContext.Response.Body, Encoding.UTF8, 4096, true);

            writer.Write("data:initialize\n\n");
            await writer.FlushAsync();

            for (int i = 0; i < 10; i++)
            {
                writer.Write("data:test + " + i + " \n\n");
                writer.FlushAsync();
                Thread.Sleep(1000);
            }
        }
    }
}
