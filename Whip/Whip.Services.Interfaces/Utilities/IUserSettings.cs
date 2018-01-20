using System;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IUserSettings
    {
        event Action ScrobblingStatusChanged;
        event Action ShufflingStatusChanged;

        bool EssentialSettingsSet { get; }
        string LastFmApiSessionKey { get; set; }
        string LastFmUsername { get; set; }
        string MusicDirectory { get; set; }
        string ArchiveDirectory { get; set; }
        string MainColourRgb { get; set; }
        bool Scrobbling { get; set; }
        bool ShuffleOn { get; set; }
        
        Task SaveAsync();
        Task SetStartupDefaultsAsync();

        string DataDirectory { get; }
    }
}
