using Hangfire.Server;
using System;

namespace HangfireWebAppSample.Interfaces
{
    public interface IDelayedJob : IJob
    {
        TimeSpan? Delay { get; }
        DateTimeOffset? EnqueueAt { get; }
    }
}
