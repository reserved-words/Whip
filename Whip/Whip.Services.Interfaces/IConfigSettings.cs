using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IConfigSettings
    {
        string LastFmApiKey { get; }
        string LastFmApiSecret { get; }
        string BandsInTownApiKey { get; }
        string YouTubeApiKey { get; }
        List<string> FileExtensions { get; }
        int TrackChangeDelay { get; }
        int MinutesBeforeRefreshNews { get; }
        int NumberOfSimilarArtistsToDisplay { get; }
        int DaysBeforeUpdatingArtistWebInfo { get; }
    }
}
