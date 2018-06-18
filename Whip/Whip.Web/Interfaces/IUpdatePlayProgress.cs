using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Interfaces
{
    public interface IUpdatePlayProgress
    {
        void StartNewTrack(int secondsPlayed, int duration);
        void UpdateSecondsPlayed(int secondsPlayed);
        void UpdatePercentagePlayed(double percentage);
        void Stop(int secondsPlayed);
    }
}