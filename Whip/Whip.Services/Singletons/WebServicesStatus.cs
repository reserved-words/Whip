using Whip.Services.Interfaces.Singletons;

namespace Whip.Services.Singletons
{
    public class WebServicesStatus : IWebServicesStatus
    {
        public WebServicesStatus()
        {
            InternetStatus = true;
            BandsInTownStatus = true;
            LastFmStatus = true;
            TwitterStatus = true;
            YouTubeStatus = true;
        }

        public bool LastFmStatus { get; private set; }
        public string LastFmErrorMessage { get; private set; }
        public string BandsInTownErrorMessage { get; private set; }
        public bool BandsInTownStatus { get; private set; }
        public bool InternetStatus { get; private set; }
        public string YouTubeErrorMessage { get; private set; }
        public bool YouTubeStatus { get; private set; }
        public string TwitterErrorMessage { get; private set; }
        public bool TwitterStatus { get; private set; }

        public void SetBandsInTownStatus(bool online, string errorMessage = null)
        {
            BandsInTownStatus = online;
            BandsInTownErrorMessage = errorMessage;
        }

        public void SetInternetStatus(bool online)
        {
            InternetStatus = online;
        }

        public void SetLastFmStatus(bool online, string errorMessage = null)
        {
            LastFmStatus = online;
            LastFmErrorMessage = errorMessage;
        }

        public void SetTwitterStatus(bool online, string errorMessage = null)
        {
            TwitterStatus = online;
            TwitterErrorMessage = errorMessage;
        }

        public void SetYouTubeStatus(bool online, string errorMessage = null)
        {
            YouTubeStatus = online;
            YouTubeErrorMessage = errorMessage;
        }
    }
}
