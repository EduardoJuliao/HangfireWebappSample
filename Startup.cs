using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Hangfire.Common;
using Hangfire.MemoryStorage;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using HangfireWebAppSample.Interfaces;
using HangfireWebAppSample.Extensions;
using HangfireWebAppSample.Interfaces.Lifecycle;

namespace HangfireWebAppSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseSerilogLogProvider()
                .UseMemoryStorage()
                .UseConsole());

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddHangfireConsoleExtensions();

            services.AddMvc();

            var jobs = typeof(IJob).GetClassesAssignableFrom();

            foreach (Type jobType in jobs)
            {
                var interfaces = jobType.GetInterfaces().ToList();
                if (interfaces.Contains(typeof(ISingletonLifecycle)))
                    services.AddSingleton(jobType);
                else if (interfaces.Contains(typeof(ITransientLifecycle)))
                    services.AddTransient(jobType);
                else if (interfaces.Contains(typeof(IScopedLifecycle)))
                    services.AddScoped(jobType);
                else
                    services.AddScoped(jobType);
            }
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard("/dashboard");
            });

            serviceProvider.ConfigureFireOnce(backgroundJobClient);
            serviceProvider.ConfigureDelayed(backgroundJobClient);
            serviceProvider.ConfigureRecurring(recurringJobManager);
        }
    }
}
