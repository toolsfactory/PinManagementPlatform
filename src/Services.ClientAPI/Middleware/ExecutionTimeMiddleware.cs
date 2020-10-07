using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PinPlatform.Services.ClientAPI.Middleware
{
    public class ExecutionTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ExecutionTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();
                context.Response.Headers.Append("X-Dev-ExecutionTime-ms", stopwatch.ElapsedMilliseconds.ToString());
                context.Response.Headers.Append("X-Dev-TraceId", context.TraceIdentifier);
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}