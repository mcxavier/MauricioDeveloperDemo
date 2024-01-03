namespace PortalParceiroHangfire.SchedulesLogger
{
    public interface IScheduleLogger
    {
        void SetError(string message);

        void SetError(string message, string stackTrace);

        void SetInfo(string message);

        void SetProgress(int progress);

        void SetSuccess(string message);

        void SetWarning(string message);
    }
}