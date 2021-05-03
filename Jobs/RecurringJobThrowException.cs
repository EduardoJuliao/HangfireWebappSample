using Hangfire.Server;
using HangfireWebAppSample.Interfaces;

namespace HangfireWebAppSample.Jobs
{
    public class RecurringJobThrowException : IRecurringJob
    {
        public void Work(PerformContext context)
        {
            throw new System.NotImplementedException("This exception is supposed to show an example on how exceptions works in Hangfire");
        }

        public string JobId { get; } = "Recurring job that throws exception";
        public string CronExpression { get; } = "5 * * * * *";
        public bool TriggerOnStartup { get; } = true;
    }
}