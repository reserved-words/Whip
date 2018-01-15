using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces.Singletons
{
    public interface IWebServicesStatus
    {
        bool LastFmStatus { get; set; }
        string LastFmErrorMessage { get; }
        bool Offline { get; set; }

        void SetInternetStatus(bool online);
        void TurnOffLastFm(string errorMessage);
    }
}
