using System.Threading;

namespace PortalParceiroHangfire.CancellationTokens
{
    public interface IJobCancellation
    {
        CancellationToken GetToken();
        bool IsCancellationRequested();
        void ThrowSeCancelado();
    }
}
