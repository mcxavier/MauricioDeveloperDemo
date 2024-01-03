using Hangfire;
using System;
using Unity;

namespace PortalParceiroHangfire.JobsActivators
{
    internal class UnityJobActivatorScope : JobActivatorScope
    {
        internal readonly IUnityContainer Container;

        internal UnityJobActivatorScope(IUnityContainer unityContainer)
        {
            Container = unityContainer;
        }

        public override void DisposeScope()
        {
            Container.Dispose();
            base.DisposeScope();
        }

        public override object Resolve(Type type)
        {
            return Container.Resolve(type);
        }
    }
}
