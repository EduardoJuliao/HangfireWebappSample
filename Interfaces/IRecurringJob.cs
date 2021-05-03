using Hangfire.Server;

namespace HangfireWebAppSample.Interfaces
{
    public interface IRecurringJob : IJob
    {
        string JobId { get; }
        string CronExpression { get; }
        bool TriggerOnStartup { get; }
    }
}
