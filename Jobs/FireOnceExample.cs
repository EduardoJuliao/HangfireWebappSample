using Hangfire.Server;
using HangfireWebAppSample.Interfaces;
using Microsoft.Extensions.Logging;

namespace HangfireWebAppSample.Jobs
{
    public class FireOnceExample : IFireOnceJob
    {
        private readonly ILogger<FireOnceExample> _logger;

        public FireOnceExample(ILogger<FireOnceExample> logger)
        {
            _logger = logger;
        }

        public void Work(PerformContext context)
        {
            _logger.LogInformation("This is a Fire Once and forget example.");
        }
    }
}
