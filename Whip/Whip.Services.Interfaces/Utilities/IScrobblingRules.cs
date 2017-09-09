using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IScrobblingRules
    {
        int MinimumUpdateNowPlayingDuration { get; }
        bool CanScrobble(double totalSeconds, double playedSeconds);
    }
}
