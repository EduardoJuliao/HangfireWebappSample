using Hangfire.Server;
using HangfireWebAppSample.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireWebAppSample.Jobs
{
    public class DelayedJobExample : IDelayedJob
    {
        private readonly ILogger<DelayedJobExample> _logger;

        public DelayedJobExample(ILogger<DelayedJobExample> logger)
        {
            _logger = logger;
        }

        public TimeSpan? Delay => TimeSpan.FromMinutes(3);

        public DateTimeOffset? EnqueueAt => throw new NotImplementedException();

        public void Work(PerformContext context)
        {
            _logger.LogInformation("Delayed job called!");
        }
    }
}
