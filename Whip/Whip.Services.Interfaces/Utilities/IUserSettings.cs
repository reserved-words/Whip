using System;

namespace Whip.Services.Interfaces
{
    public interface IUserSettings
    {
        event Action ScrobblingStatusChanged;

        bool EssentialSettingsSet { get; }
        string LastFmApiKey { get; set; }
        string LastFmApiSecret { get; set; }
        string LastFmApiSessionKey { get; set; }
        string LastFmUsername { get; set; }
        string MusicDirectory { get; set; }
        string MainColourRgb { get; set; }
        bool Scrobbling { get; set; }
        void Save();
    }
}
