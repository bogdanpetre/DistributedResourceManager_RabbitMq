using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceApp.Actors
{
    public interface ISignalGenerator : IDisposable
    {
        void Initialize(TimeSpan period);
        event Action Tick;
        void Start();
        void Stop();
    }

    public sealed class TimeSignalGenerator : ISignalGenerator
    {
        private readonly Timer _timer;
        private TimeSpan _period;
        private bool _isInitialized;
        private bool _isStarted;
        //TODO: make it thread safe

        public TimeSignalGenerator()
        {
            _timer = new Timer(_ => GenerateSignal());
            _isInitialized = false;
            _isStarted = false;
        }

        #region ISignalGenerator Members

        public void Initialize(TimeSpan period)
        {
            Stop();
            _period = period;
            _timer.Change(TimeSpan.FromSeconds(0), _period);
            _isInitialized = true;
        }

        public event Action Tick;

        public void Start()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("You must initialize the object first.");

            _isStarted = true;
        }

        public void Stop()
        {
            _isStarted = false;
        }

        #endregion

        #region IDisposable Members

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManagedResource)
        {
            if (_disposed)
                return;

            if (disposeManagedResource)
            {
                _timer.Dispose();
            }

            _disposed = true;
        }

        #endregion

        #region Helpers

        private void GenerateSignal()
        {
            if (_isStarted)
            {
                var handler = Tick;
                if (handler != null)
                    handler();
            }
        }

        #endregion
    }
}
