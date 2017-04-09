using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class ScrobblingRulesService : IScrobblingRulesService
    {
        private const double MaxSecondsBeforeScrobbling = 240;

        public bool CanScrobble(double totalSeconds, double playedSeconds)
        {
            return playedSeconds > MaxSecondsBeforeScrobbling || playedSeconds > (totalSeconds / 2);
        }
    }
}
