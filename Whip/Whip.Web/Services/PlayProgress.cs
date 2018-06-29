using System;
using Whip.Web.Interfaces;

namespace Whip.Web.Services
{
    public class PlayProgress : IPlayProgress, IUpdatePlayProgress
    {
        public int PreviousDuration { get; private set; }
        public int PreviousSecondsPlayed { get; private set; }
        public int CurrentDuration { get; private set; }
        public int CurrentSecondsPlayed { get; private set; }

        public int CurrentSecondsRemaining => CurrentDuration - CurrentSecondsPlayed;
        
        public void StartNewTrack(int secondsPlayed, int duration)
        {
            PreviousDuration = CurrentDuration;
            PreviousSecondsPlayed = secondsPlayed;
            CurrentDuration = duration;
            CurrentSecondsPlayed = 0;
        }

        public void UpdateSecondsPlayed(int totalSeconds)
        {
            CurrentSecondsPlayed = Math.Min(totalSeconds, CurrentDuration);
        }

        public void UpdatePercentagePlayed(double percentage)
        {
            CurrentSecondsPlayed = (int)Math.Round(percentage * CurrentDuration / 100, 0);
        }

        public void Stop(int secondsPlayed)
        {
            PreviousDuration = CurrentDuration;
            PreviousSecondsPlayed = secondsPlayed;
            CurrentDuration = 0;
            CurrentSecondsPlayed = 0;
        }
    }
}