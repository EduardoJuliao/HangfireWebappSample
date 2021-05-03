using Hangfire;
using HangfireWebAppSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireWebAppSample.Extensions
{
    public static class ServiceProviderJobExtension
    {
        public static IServiceProvider ConfigureFireOnce(this IServiceProvider serviceProvider, IBackgroundJobClient backgroundJobClient)
        {
            var classes = typeof(IFireOnceJob).GetClassesAssignableFrom();

            foreach (Type t in classes)
                backgroundJobClient.Enqueue(() => (serviceProvider.GetService(t) as IFireOnceJob).Work(null));

            return serviceProvider;
        }

        public static IServiceProvider ConfigureDelayed(this IServiceProvider serviceProvider, IBackgroundJobClient backgroundJobClient)
        {
            var classes = typeof(IDelayedJob).GetClassesAssignableFrom();

            foreach (Type t in classes)
            {
                var delayed = (IDelayedJob)serviceProvider.GetService(t);
                if (!delayed.Delay.HasValue && !delayed.EnqueueAt.HasValue)
                {
                    throw new NotImplementedException($"Hey! Delayed job {delayed.GetType()} don't have a Deley or Enqueue At value! please check!");
                }

                if (delayed.Delay.HasValue)
                    backgroundJobClient.Schedule(() => delayed.Work(null), delayed.Delay.Value);
                else if (delayed.EnqueueAt.HasValue)
                    backgroundJobClient.Schedule(() => delayed.Work(null), delayed.EnqueueAt.Value);

            }

            return serviceProvider;
        }

        public static IServiceProvider ConfigureRecurring(this IServiceProvider serviceProvider, IRecurringJobManager recurringJobManager)
        {
            var classes = typeof(IRecurringJob).GetClassesAssignableFrom();

            foreach (Type t in classes)
            {
                var job = (IRecurringJob)serviceProvider.GetService(t);

                recurringJobManager.RemoveIfExists(job.JobId);

                recurringJobManager.AddOrUpdate(job.JobId, () => job.Work(null), job.CronExpression);
                if (job.TriggerOnStartup)
                    recurringJobManager.Trigger(job.JobId);
            }

            return serviceProvider;
        }

        public static IServiceProvider ConfigureContinuations(this IServiceProvider serviceProvider, IBackgroundJobClient backgroundJobClient)
        {
            var continuations = typeof(IContinuationJob).GetClassesAssignableFrom();

            foreach (Type t in continuations)
            {
                var continuation = (IContinuationJob)serviceProvider.GetService(t);
                backgroundJobClient.ContinueJobWith(continuation.ParentJobId, () => continuation.Work(null), JobContinuationOptions.OnlyOnSucceededState);
            }

            return serviceProvider;
        }
    }
}
