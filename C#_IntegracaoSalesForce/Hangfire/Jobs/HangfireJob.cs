using Hangfire.Server;
using PortalParceiroHangfire.CancellationTokens;
using PortalParceiroHangfire.EnqueuedJobLogger;
using PortalParceiroHangfire.Filters;
using PortalParceiroHangfire.JobsActivators;
using Unity;

namespace PortalParceiroHangfire.Jobs
{
    public abstract class HangfireJob : IHangfireJob
    {
        protected string JobId => GetJobId();
        protected PerformingContext PerformingContext => GetContext();
        protected IUnityContainer UnityJobContainer => GetUnityJobContainer();

        public void Execute()
        {
            var cancellationToken = PerformingContext?.CancellationToken;
            Do(new JobLogger(PerformingContext), new JobCancellation(cancellationToken));
        }
        private string GetJobId()
        {
            var context = JobContext.Context;
            return context?.BackgroundJob?.Id;
        }

        protected abstract void Do(IJobLogger logger, IJobCancellation cancellationToken);

        private PerformingContext GetContext() => JobContext.Context;
        private IUnityContainer GetUnityJobContainer() => UnityJobActivator.JobScope?.Container;
    }
}
