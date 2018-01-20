using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Whip.Common.Exceptions;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class YouTubeVideoService : IVideoService
    {
        private const int AuthenticationErrorCode = 403;
        private const string UploadsPlaylistUrl = "https://www.googleapis.com/youtube/v3/channels?part={0}&forUsername={1}&key={2}";
        private const string PlaylistVideosUrl = "https://www.googleapis.com/youtube/v3/playlistItems?part={0}&maxResults={1}&playlistId={2}&key={3}&order={4}";
        private const string UrlFormat = "https://www.youtube.com/v/{0}&hl=en";

        private readonly IConfigSettings _configSettings;
        private readonly IWebHelperService _webHelperService;
        
        public YouTubeVideoService(IWebHelperService webHelperService, IConfigSettings configSettings)
        {
            _configSettings = configSettings;
            _webHelperService = webHelperService;
        }

        public async Task<bool> PopulateLatestVideoAsync(Artist artist)
        {
            if (string.IsNullOrEmpty(artist.YouTube))
                return false;
            
            try
            {
                var url = string.Format(UploadsPlaylistUrl, "contentDetails", artist.YouTube, _configSettings.YouTubeApiKey);

                var response = await _webHelperService.HttpGetAsync(url);
                dynamic parsed = JObject.Parse(response);

                HandleErrors(parsed.error);

                var items = parsed.items;
                var playlistId = items.First.contentDetails.relatedPlaylists.uploads;

                if (playlistId == null)
                    return false;

                url = string.Format(PlaylistVideosUrl, "snippet", 1, playlistId, _configSettings.YouTubeApiKey, "date");
                response = await _webHelperService.HttpGetAsync(url);
                dynamic parsedResponse = JObject.Parse(response);

                var videos = parsedResponse.items;
                foreach (var video in videos)
                {
                    var snippet = video.snippet;

                    if (snippet != null)
                    {
                        artist.LatestVideo = new Video
                        {
                            Title = snippet.title,
                            Url = string.Format(UrlFormat, snippet.resourceId.videoId),
                            Published = snippet.publishedAt
                        };
                        return true;
                    }
                }
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unexpected error fetching videos", ex);
            }

            return false;
        }

        private static void HandleErrors(dynamic error)
        {
            if (error == null)
                return;

            var code = (int)error.code;

            var errorMessage = new StringBuilder();

            var reasons = new List<string>();

            foreach (var err in error.errors)
            {
                reasons.Add((string)err.reason);

                if (errorMessage.Length > 0)
                {
                    errorMessage.Append("|");
                }

                errorMessage.Append($"domain: {err.domain ?? ""}, ");
                errorMessage.Append($"reason: {err.reason ?? ""}, ");
                errorMessage.Append($"message: {err.message ?? error.message ?? ""}");
            }

            throw IsAuthenticationError(code, reasons)
                ? new ServiceAuthenticationException(errorMessage.ToString(), null)
                : new ServiceException(errorMessage.ToString(), null);
        }

        private static bool IsAuthenticationError(int code, ICollection<string> reasons)
        {
            return code == AuthenticationErrorCode || reasons.Contains("keyInvalid");
        }
    }
}
