using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ILibraryDataOrganiserService
    {
        void AddTrack(string filepath, File file, ICollection<Artist> artists);

        void SyncTracks(ICollection<Artist> artists, ICollection<string> filepathsToKeep);

        void ReorganiseOnTrackChange(Track trackChanged, Artist originalArtist, Disc originalDisc);
    }
}
