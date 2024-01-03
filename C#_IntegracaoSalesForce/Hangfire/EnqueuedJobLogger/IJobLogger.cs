namespace PortalParceiroHangfire.EnqueuedJobLogger
{
    public interface IJobLogger
    {
        void SetError(string message);

        void SetError(string message, string stackTrace);

        void SetInfo(string message);

        void SetProgress(int progress);

        void SetSuccess(string message);

        void SetSystemInfo(string message);

        void SetWarning(string message);
    }
}
