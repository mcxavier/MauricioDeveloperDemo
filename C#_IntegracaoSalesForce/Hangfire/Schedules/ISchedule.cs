using Hangfire.Server;

namespace PortalParceiroHangfire.Schedules
{
    public interface ISchedule
    {
        void Execute(PerformContext context);
    }
}