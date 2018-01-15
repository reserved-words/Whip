using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces.Singletons
{
    public interface IWebServicesStatus
    {
        string BandsInTownErrorMessage { get; }
        bool BandsInTownStatus { get; }
        bool InternetStatus { get; }
        string LastFmErrorMessage { get; }
        bool LastFmStatus { get; }
        string YouTubeErrorMessage { get; }
        bool YouTubeStatus { get; }
        string TwitterErrorMessage { get; }
        bool TwitterStatus { get; }
        
        void SetInternetStatus(bool online);
        void SetBandsInTownStatus(bool online, string errorMessage = null);
        void SetLastFmStatus(bool online, string errorMessage = null);
        void SetTwitterStatus(bool online, string errorMessage = null);
        void SetYouTubeStatus(bool online, string errorMessage = null);
    }
}
