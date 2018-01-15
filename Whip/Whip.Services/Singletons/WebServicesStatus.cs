using Whip.Services.Interfaces.Singletons;

namespace Whip.Services.Singletons
{
    public class WebServicesStatus : IWebServicesStatus
    {
        private string _lastFmErrorMessage;
        private bool _lastFmStatus = true;
        private bool _offline = false;

        public bool LastFmStatus
        {
            get { return _lastFmStatus; }
            set { _lastFmStatus = value; }
        }

        public bool Offline
        {
            get { return _offline; }
            set { _offline = value; }
        }

        public string LastFmErrorMessage
        {
            get { return _lastFmErrorMessage; }
            private set { _lastFmErrorMessage = value; }
        }

        public void SetInternetStatus(bool online)
        {
            Offline = !online;
        }

        public void TurnOffLastFm(string errorMessage)
        {
            LastFmStatus = false;
            LastFmErrorMessage = errorMessage;
        }
    }
}
