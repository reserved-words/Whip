using LastFmApi;

namespace Whip.LastFm
{
    public interface IErrorHandlingService
    {
        void HandleError(LastFmApiException exception, string additionalInfo = "");
    }
}
