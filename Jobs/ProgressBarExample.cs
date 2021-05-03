using Hangfire.Console;
using Hangfire.Server;
using HangfireWebAppSample.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HangfireWebAppSample.Jobs
{
    public class ProgressBarExample : IFireOnceJob
    {
        private readonly ILogger<ProgressBarExample> _logger;

        private readonly List<int> _mockDataToProcess = Enumerable.Range(0, short.MaxValue).Select(x => x).ToList();

        public ProgressBarExample(ILogger<ProgressBarExample> logger)
        {
            _logger = logger;
        }

        public void Work(PerformContext context)
        {
            _logger.LogInformation("Started progress bar example");

            var progressBar = context.WriteProgressBar();

            for(var i = 0; i < _mockDataToProcess.Count; i++)
            {
                if (i % 2500 == 0)
                {
                    _logger.LogInformation("Simulating a data send...");
                    Thread.Sleep(2000);
                }

                progressBar.SetValue(((double)i / _mockDataToProcess.Count) * 100);
            }

            _logger.LogInformation("Really long execution finished.");
        }
    }
}
