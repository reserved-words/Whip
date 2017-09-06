using GalaSoft.MvvmLight;
using System;
using System.Threading;
using Timer = System.Timers.Timer;
using Whip.Common.Model;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.Utilities
{
    public class TrackTimer : ViewModelBase, IDisposable
    {
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        private readonly Timer _timer = new Timer(1000);

        private int _trackDurationInSeconds;
        private int _secondsPlayed;

        private int _percentagePlayed;
        private string _timePlayed;
        private string _timeToPlay;

        public TrackTimer()
        {
            _timer.Elapsed += TimerElapsed;

            SetProperties();
        }

        public event Action TrackEnded;
        
        public int PercentagePlayed
        {
            get { return _percentagePlayed; }
            set { Set(ref _percentagePlayed, value); }
        }

        public string TimePlayed
        {
            get { return _timePlayed; }
            set { Set(ref _timePlayed, value); }
        }

        public string TimeToPlay
        {
            get { return _timeToPlay; }
            set { Set(ref _timeToPlay, value); }
        }

        public virtual void Reset(Track track)
        {
            _secondsPlayed = 0;
            _trackDurationInSeconds = (int)(track?.Duration.TotalSeconds ?? 0);
            SetProperties();
        }

        public virtual void SkipToPercentage(double newPercentage)
        {
            _secondsPlayed = (int)((newPercentage / 100) * _trackDurationInSeconds);
            SetProperties();
        }

        public virtual void Start()
        {
            _timer.Start();
        }

        public virtual void Stop()
        {
            _timer.Stop();
        }

        private string GetTimeSpanFormat(double seconds, bool negative)
        {
            return (negative ? "-" : "") + TimeSpan.FromSeconds(seconds).ToString(StandardTimeSpanFormat);
        }

        private void SetProperties()
        {
            PercentagePlayed = _trackDurationInSeconds == 0
                ? 0
                : Convert.ToInt16(100 * _secondsPlayed / _trackDurationInSeconds);

            TimePlayed = GetTimeSpanFormat(_secondsPlayed, false);
            TimeToPlay = GetTimeSpanFormat(_trackDurationInSeconds - _secondsPlayed, true);
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _secondsPlayed++;

            SetProperties();

            if (_secondsPlayed >= _trackDurationInSeconds)
            {
                _timer.Stop();

                _synchronizationContext.Send(state =>
                {
                    TrackEnded?.Invoke();
                }
                , null);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
