using Hangfire;
using System;
using Unity;

namespace PortalParceiroHangfire.JobsActivators
{
    internal class UnityJobActivator : JobActivator
    {
        [ThreadStatic]
        internal static UnityJobActivatorScope JobScope;

        private readonly Func<IUnityContainer> _createContainer;

        public UnityJobActivator(Func<IUnityContainer> createContainer)
        {
            _createContainer = createContainer;
        }

        public override object ActivateJob(Type jobType)
        {
            using (var container = _createContainer())
            {
                return container.Resolve(jobType);
            }
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            JobScope = new UnityJobActivatorScope(_createContainer());
            return JobScope;
        }
    }
}
