using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using System;

namespace PortalParceiroHangfire.EnqueuedJobLogger
{
    public class JobLogger : IJobLogger
    {
        private readonly PerformContext _context;
        private readonly object _lockObject = new Object();
        private readonly Lazy<IProgressBar> _progressBar;

        public JobLogger(PerformContext context)
        {
            _context = context;
            _progressBar = new Lazy<IProgressBar>(() => _context.WriteProgressBar());
        }

        public void SetError(string message)
        {
            SetError(message, "");
        }

        public void SetError(string message, string stackTrace)
        {
            lock (_lockObject)
            {
                var length = stackTrace.Length > 1300 ? 1300 : stackTrace.Length;
                _context.SetTextColor(ConsoleTextColor.Red);
                _context.WriteLine($"Erro: {message}. Stack trace: { (string.IsNullOrWhiteSpace(stackTrace) ? "(não informado)" : stackTrace.Substring(0, length))}.");
                _context.ResetTextColor();
            }
        }

        public void SetInfo(string message)
        {
            lock (_lockObject)
            {
                _context.WriteLine($"Info: {message}");
            }
        }

        public void SetProgress(int progress)
        {
            lock (_lockObject)
            {
                _progressBar.Value.SetValue(progress);
            }
        }

        public void SetSuccess(string message)
        {
            lock (_lockObject)
            {
                _context.SetTextColor(ConsoleTextColor.Green);
                _context.WriteLine($"Sucesso: {message}");
                _context.ResetTextColor();
            }
        }

        public void SetSystemInfo(string message)
        {
            lock (_lockObject)
            {
                _context.SetTextColor(ConsoleTextColor.Blue);
                _context.WriteLine($"System: {message}");
                _context.ResetTextColor();
            }
        }

        public void SetWarning(string message)
        {
            lock (_lockObject)
            {
                _context.SetTextColor(ConsoleTextColor.Yellow);
                _context.WriteLine($"Warning: {message}");
                _context.ResetTextColor();
            }
        }
    }
}
