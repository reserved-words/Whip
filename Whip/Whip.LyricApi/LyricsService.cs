using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web;
using Whip.Services.Interfaces;

namespace Whip.LyricApi
{
    public class LyricsService : ILyricsService
    {
        private const string LyricApiUrl = "https://lyric-api.herokuapp.com/api/find/{0}/{1}";

        private readonly IWebHelperService _webHelperService;

        public LyricsService(IWebHelperService webHelperService)
        {
            _webHelperService = webHelperService;
        }

        public async Task<string> GetLyrics(string artistName, string trackTitle)
        {
            var artist = HttpUtility.HtmlEncode(artistName);
            var track = HttpUtility.HtmlEncode(trackTitle);

            var url = string.Format(LyricApiUrl, artist, track);

            var response = await _webHelperService.HttpGetAsync(url);
            dynamic parsed = JObject.Parse(response);

            return parsed.err != "none"
                ? null
                : parsed.lyric;
        }
    }
}
