using Hangfire.Server;
using System;

namespace PortalParceiroHangfire.Filters
{
    public class JobContext : IServerFilter
    {
        [ThreadStatic]
        private static PerformingContext _context;

        public static PerformingContext Context { get { return _context; } set { _context = value; } }

        public void OnPerformed(PerformedContext filterContext)
        {
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            Context = filterContext;
        }
    }
}
