using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SampleWebApp
{
    internal class CustomMiddleWare
    {
        public CustomMiddleWare(RequestDelegate next,IHostingEnvironment env)
        {
            this.Next = next;
            this.EnvironmentName = env.EnvironmentName;
        }

        public RequestDelegate Next { get; }
        public string EnvironmentName { get; }

        public async Task Invoke(HttpContext context)
        {
            var timer = Stopwatch.StartNew();
            context.Response.Headers.Add("x-HostingEnvironmentName", new[] { EnvironmentName });
        }
    }
}