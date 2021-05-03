using Hangfire.Server;
using HangfireWebAppSample.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace HangfireWebAppSample.Jobs
{
    public class RecurringJobExample : IRecurringJob
    {
        private readonly ILogger<RecurringJobExample> _logger;

        public RecurringJobExample(ILogger<RecurringJobExample> logger)
        {
            _logger = logger;
        }

        public string JobId => "Recurring Job Example";
        public string CronExpression => "*/5 * * * *";

        public bool TriggerOnStartup => true;

        public void Work(PerformContext context)
        {
            _logger.LogInformation("Started");
            var counter = 0;

            do
            {
                counter++;

                _logger.LogInformation("Wait 1 second.");
                _logger.LogInformation("Current iteration of counter: {counter}.", counter);

                Thread.Sleep(1000);
            } while (counter < 4);

            _logger.LogInformation("Finished.");
        }
    }
}
