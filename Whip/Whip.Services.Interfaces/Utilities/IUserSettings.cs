﻿using System;

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
        string MainColourRgb { get; set; }
        bool Scrobbling { get; set; }
        bool ShuffleOn { get; set; }
        void Save();
    }
}