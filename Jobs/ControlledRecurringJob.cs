using System;
using System.Threading;
using Hangfire.Server;
using HangfireWebAppSample.Interfaces;
using HangfireWebAppSample.Interfaces.Lifecycle;
using Microsoft.Extensions.Logging;

namespace HangfireWebAppSample.Jobs
{
    public class ControlledRecurringJob : IRecurringJob, ISingletonLifecycle
    {
        private readonly ILogger<ControlledRecurringJob> _logger;

        private bool _isProcessing = false;

        public ControlledRecurringJob(ILogger<ControlledRecurringJob> logger)
        {
            _logger = logger;
        }
        
        public void Work(PerformContext context)
        {
            if (_isProcessing)
            {
                _logger.LogInformation("Can't start new process since previous haven't finished.");
                return;
            }

            try
            {
                _isProcessing = true;
                var counter = 0;

                do
                {
                    counter++;

                    _logger.LogInformation("Wait 1 second.");
                    _logger.LogInformation("Current iteration of counter: {counter}.", counter);

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                } while (counter < 5);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        public string JobId { get; } = "Controlled Recurring Job Example";
        public string CronExpression { get; } = "5 * * * * *";
        public bool TriggerOnStartup { get; } = true;
    }
}