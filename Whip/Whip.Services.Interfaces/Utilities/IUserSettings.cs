using System;

namespace Whip.Services.Interfaces
{
    public interface IUserSettings
    {
        event Action ScrobblingStatusChanged;
        event Action ShufflingStatusChanged;

        bool EssentialSettingsSet { get; }
        string LastFmApiKey { get; set; }
        string LastFmApiSecret { get; set; }
        string LastFmApiSessionKey { get; set; }
        string LastFmUsername { get; set; }
        string MusicDirectory { get; set; }
        string ArchiveDirectory { get; set; }
        string MainColourRgb { get; set; }
        bool Scrobbling { get; set; }
        bool ShuffleOn { get; set; }
        bool LastFmStatus { get; set; }
        string LastFmErrorMessage { get; }
        bool Offline { get; set; }

        void Save();
        void SetInternetStatus(bool online);
        void TurnOffLastFm(string errorMessage);
        void SetStartupDefaults();
    }
}
