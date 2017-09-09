using System;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class PlayTimer : IPlayTimer
    {
        private readonly ICurrentDateTime _currentDateTime;

        private DateTime _timeStartedPlaying;
        private int _secondsPlayed;

        public PlayTimer(ICurrentDateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;
        }

        public int RemainingSeconds { get; private set; }
        public int TotalTrackDurationInSeconds { get; private set; }
        public int SecondsOfTrackPlayed => TotalTrackDurationInSeconds - RemainingSeconds;

        public void StartNewTrack(int durationInSeconds)
        {
            _timeStartedPlaying = _currentDateTime.Get();
            _secondsPlayed = 0;
            TotalTrackDurationInSeconds = durationInSeconds;
            RemainingSeconds = TotalTrackDurationInSeconds;
        }

        public void Pause()
        {
            _secondsPlayed = _secondsPlayed + (int)(_currentDateTime.Get() - _timeStartedPlaying).TotalSeconds;
            RemainingSeconds = TotalTrackDurationInSeconds - _secondsPlayed;
        }

        public void Resume()
        {
            _timeStartedPlaying = _currentDateTime.Get();
        }

        public void SkipToPercentage(double percentage)
        {
            RemainingSeconds = (int)(percentage / 100) * TotalTrackDurationInSeconds;
            _secondsPlayed = TotalTrackDurationInSeconds - RemainingSeconds;
        }
    }
}
