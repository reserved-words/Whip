using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IPlayProgressTracker
    {
        int RemainingSeconds { get; }
        int TotalTrackDurationInSeconds { get; }
        int SecondsOfTrackPlayed { get; }
        void StartNewTrack(int durationInSeconds);
        void Pause();
        void Resume();
        void SkipToPercentage(double percentage);
    }
}
