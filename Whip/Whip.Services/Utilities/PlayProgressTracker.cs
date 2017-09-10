using System;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class PlayProgressTracker : IPlayProgressTracker
    {
        private readonly ICurrentDateTime _currentDateTime;

        private DateTime _timeStartedPlaying;

        public PlayProgressTracker(ICurrentDateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;
        }

        public int TotalTrackDurationInSeconds { get; private set; }
        public int SecondsOfTrackPlayed { get; private set; }
        public int RemainingSeconds => TotalTrackDurationInSeconds - SecondsOfTrackPlayed;

        public void StartNewTrack(int durationInSeconds)
        {
            _timeStartedPlaying = _currentDateTime.Get();
            SecondsOfTrackPlayed = 0;
            TotalTrackDurationInSeconds = durationInSeconds;
        }

        public void Pause()
        {
            SecondsOfTrackPlayed = SecondsOfTrackPlayed + (int)(_currentDateTime.Get() - _timeStartedPlaying).TotalSeconds;
        }

        public void Resume()
        {
            _timeStartedPlaying = _currentDateTime.Get();
        }

        public void SkipToPercentage(double percentage)
        {
            _timeStartedPlaying = _currentDateTime.Get();
            SecondsOfTrackPlayed = (int)(TotalTrackDurationInSeconds * percentage / 100);
        }
    }
}
