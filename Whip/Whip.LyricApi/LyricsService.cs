using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Whip.Common.Enums;
using Whip.Services.Interfaces;

namespace Whip.LyricApi
{
    public class LyricsService : ILyricsService
    {
        private const string LyricApiUrl = "https://lyric-api.herokuapp.com/api/find/{0}/{1}";

        private readonly IAsyncMethodInterceptor _interceptor;
        private readonly IWebHelperService _webHelperService;
        private readonly ILoggingService _loggingService;

        public LyricsService(IWebHelperService webHelperService, IAsyncMethodInterceptor interceptor, ILoggingService loggingService)
        {
            _interceptor = interceptor;
            _loggingService = loggingService;
            _webHelperService = webHelperService;
        }

        public async Task<string> GetLyrics(string artistName, string trackTitle)
        {
            var artist = HtmlEncode(artistName, true);
            var track = HtmlEncode(trackTitle, false);

            var url = string.Format(LyricApiUrl, artist, track);

            var response = await _interceptor.TryMethod(_webHelperService.HttpGetAsync(url), null, WebServiceType.Lyrics, "Getting lyrics from " + url);

            JsonReaderException exception = null;
            
            if (response != null)
            {
                try
                {
                    dynamic parsedResponse = JObject.Parse(response);

                    if (parsedResponse.err == "none")
                    {
                        return parsedResponse.lyric;
                    }
                }
                catch (JsonReaderException ex)
                {
                    exception = ex;
                }
            }

            LogError(url, response, exception);
            return null;
        }

        private static string HtmlEncode(string str, bool removeQuestionMark)
        {
            return HttpUtility.HtmlEncode(removeQuestionMark ? str.Replace("?","") : str);
        }

        private void LogError(string url, string response, JsonReaderException ex = null)
        {
            var message =  response == null
                ? $"No response when fetching lyrics from {url}"
                : $"Invalid response when fetching lyrics from {url}: {response}";

            _loggingService.Warn(message);

            if (ex != null)
            {
                _loggingService.Warn(ex.Message);
            }
        }
    }
}
