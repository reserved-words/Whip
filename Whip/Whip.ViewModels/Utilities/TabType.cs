using Whip.Common;
using Whip.Common.Utilities;

namespace Whip.ViewModels.Utilities
{
    public enum TabType
    {
        [MetaData("Archive", IconType.Archive)]
        Archive,
        [MetaData("Library", IconType.Book)]
        Library,
        [MetaData("Settings", IconType.Cog)]
        Settings,
        [MetaData("Dashboard", IconType.Home)]
        Dashboard,
        [MetaData("Last.FM", IconType.LastFmSquare)]
        LastFm,
        [MetaData("Current Playlist", IconType.ListOl)]
        CurrentPlaylist,
        [MetaData("Playlists", IconType.ListUl)]
        Playlists,
        [MetaData("Current Track", IconType.Music)]
        CurrentTrack,
        [MetaData("News", IconType.Rss)]
        News,
        [MetaData("Library Search", IconType.Search)]
        Search,
        [MetaData("Current Artist", IconType.Users)]
        CurrentArtist
    }
}
