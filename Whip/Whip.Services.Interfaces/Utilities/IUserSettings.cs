using System;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IUserSettings : ILibrarySettings
    {
        event Action ScrobblingStatusChanged;
        event Action ShufflingStatusChanged;

        bool EssentialSettingsSet { get; }
        string LastFmApiSessionKey { get; set; }
        string LastFmUsername { get; set; }
        string MainColourRgb { get; set; }
        bool Scrobbling { get; set; }
        bool ShuffleOn { get; set; }
        
        Task SaveAsync();
        Task SetStartupDefaultsAsync();
    }
}
