using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Configuration;
using HangfireWebAppSample.Jobs;
using Hangfire.MemoryStorage;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using HangfireWebAppSample.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Hangfire.Server;
using HangfireWebAppSample.Extensions;

namespace HangfireWebAppSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

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

            foreach (Type job in jobs)
                services.AddTransient(job);
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
