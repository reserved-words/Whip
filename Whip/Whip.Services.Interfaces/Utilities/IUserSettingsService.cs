using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IUserSettingsService
    {
        bool EssentialSettingsSet { get; }
        string LastFmApiKey { get; set; }
        string LastFmApiSecret { get; set; }
        string LastFmApiSessionKey { get; set; }
        string LastFmUsername { get; set; }
        string MusicDirectory { get; set; }
        void Save();
    }
}
