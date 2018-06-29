using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Interfaces
{
    public interface IPlayProgress
    {
        int PreviousDuration { get; }
        int PreviousSecondsPlayed { get; }
        int CurrentDuration { get; }
        int CurrentSecondsPlayed { get; }
        int CurrentSecondsRemaining { get; }
    }
}