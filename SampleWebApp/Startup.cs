using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("foo.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();

            app.UseMiddleware<CustomMiddleWare>();

            app.Use(async (context, next) =>
            {
                var myTimer = System.Diagnostics.Stopwatch.StartNew();
                logger.LogInformation("MW1: Incoming request Started");
                //await context.Response.WriteAsync("Hello from middleware 1!");
                await next();
                logger.LogInformation("MW1: Outgoing Response");
                logger.LogInformation($"-- Request comepleted in {myTimer.ElapsedMilliseconds}ms for {env.EnvironmentName} env.");
            });


            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW2: Incoming request");
                //await context.Response.WriteAsync("Hello from middleware 1!");
                await next();
                logger.LogInformation("MW2: Outgoing Response");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
                //await context.Response.WriteAsync("MW3: Request handled and response produced.");
                //logger.LogInformation("MW3: Request handled and response produced.");
            });
        }
    }
}
