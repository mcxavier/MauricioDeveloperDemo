using PortalParceiroHangfire.SchedulesLogger;

namespace Schedules.SincronismoSalesforce
{
    public interface ISincronismoSalesforce
    {
        void Execute(IScheduleLogger logger);
    }
}
