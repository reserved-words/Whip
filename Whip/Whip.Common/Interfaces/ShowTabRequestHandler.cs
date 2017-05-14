using System;
using Whip.Common.Model;

namespace Whip.Common.Interfaces
{
    public interface IShowTabRequestHandler
    {
        event Action<Track> ShowEditTrackTab;
        event Action ShowSettingsTab;
        event Action<CriteriaPlaylist> ShowEditCriteriaPlaylistTab;
    }
}
