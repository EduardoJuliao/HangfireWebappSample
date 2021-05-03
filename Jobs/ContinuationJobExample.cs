using Hangfire.Server;
using HangfireWebAppSample.Interfaces;
using Microsoft.Extensions.Logging;

namespace HangfireWebAppSample.Jobs
{
    public class ContinuationJobExample : IContinuationJob
    {
        private readonly ILogger<ContinuationJobExample> _logger;

        public ContinuationJobExample(ILogger<ContinuationJobExample> logger)
        {
            _logger = logger;
        }

        public string ParentJobId => "Recurring_job_with_progress_bar";

        public void Work(PerformContext context)
        {
            _logger.LogInformation("This is a continuation job triggered by: " + context.BackgroundJob.Id);
        }
    }
}
