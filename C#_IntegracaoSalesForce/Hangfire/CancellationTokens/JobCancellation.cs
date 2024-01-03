using Hangfire;
using System.Threading;

namespace PortalParceiroHangfire.CancellationTokens
{
    public class JobCancellation : IJobCancellation
    {
        private readonly IJobCancellationToken _token;

        public JobCancellation(IJobCancellationToken token)
        {
            _token = token;
        }

        public CancellationToken GetToken() => _token.ShutdownToken;

        public bool IsCancellationRequested() =>
            _token.ShutdownToken.IsCancellationRequested;

        public void ThrowSeCancelado() =>
            _token?.ThrowIfCancellationRequested();
    }
}
