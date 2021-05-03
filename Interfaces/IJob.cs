using Hangfire.Server;

namespace HangfireWebAppSample.Interfaces
{
    public interface IJob
    {
        void Work(PerformContext context);
    }
}
