using Hangfire.Server;
using PortalParceiroHangfire.SchedulesLogger;
using System;

namespace PortalParceiroHangfire.Schedules
{
    public class Schedule : ISchedule
    {
        private readonly Action<IScheduleLogger> _action;

        public Schedule(Action<IScheduleLogger> action)
        {
            _action = action;
        }

        public void Execute(PerformContext context)
        {
            var logger = new ScheduleLogger(context);
            _action(logger);
        }
    }
}