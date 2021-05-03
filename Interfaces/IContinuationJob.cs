using Hangfire.Server;

namespace HangfireWebAppSample.Interfaces
{
    public interface IContinuationJob : IJob
    {
        string ParentJobId { get; }
    }
}
