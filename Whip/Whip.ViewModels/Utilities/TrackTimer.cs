using GalaSoft.MvvmLight;
using System;
using System.Timers;
using Whip.Common.Model;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.Utilities
{
    public class TrackTimer : ViewModelBase
    {
        private readonly Timer _timer = new Timer(1000);

        private double _trackDurationInSeconds;
        private double _secondsPlayed;

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

        public void Reset(Track track)
        {
            _secondsPlayed = 0;
            _trackDurationInSeconds = track.Duration.TotalSeconds;
            SetProperties();
        }

        public void SkipToPercentage(double newPercentage)
        {
            _secondsPlayed = (newPercentage / 100) * _trackDurationInSeconds;
            SetProperties();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _secondsPlayed++;

            SetProperties();

            if (_secondsPlayed >= _trackDurationInSeconds)
            {
                _timer.Stop();
                TrackEnded?.Invoke();
            }
        }
    }
}
