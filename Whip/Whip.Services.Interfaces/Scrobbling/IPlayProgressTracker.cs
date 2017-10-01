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
        void Resume();
        void Stop();
        void SkipToPercentage(double percentage);
    }
}
