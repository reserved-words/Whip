﻿using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class VideoService : IVideoService
    {
        private const string UploadsPlaylistUrl = "https://www.googleapis.com/youtube/v3/channels?part={0}&forUsername={1}&key={2}";
        private const string PlaylistVideosUrl = "https://www.googleapis.com/youtube/v3/playlistItems?part={0}&maxResults={1}&playlistId={2}&key={3}&order={4}";
        private const string UrlFormat = "https://www.youtube.com/v/{0}&hl=en";

        private readonly IConfigSettings _configSettings;
        private readonly IWebHelperService _webHelperService;

        public VideoService(IWebHelperService webHelperService, IConfigSettings configSettings)
        {
            _configSettings = configSettings;
            _webHelperService = webHelperService;
        }

        public async Task<Video> GetLatestVideoAsync(Artist artist)
        {
            if (string.IsNullOrEmpty(artist.YouTube))
                return null;

            var url = string.Format(UploadsPlaylistUrl, "contentDetails", artist.YouTube, _configSettings.YouTubeApiKey);
            var response = await _webHelperService.HttpGetAsync(url);
            dynamic parsed = JObject.Parse(response);
            var items = parsed.items;
            var playlistId = items.First.contentDetails.relatedPlaylists.uploads;

            if (playlistId == null)
                return null;

            url = string.Format(PlaylistVideosUrl, "snippet", 1, playlistId, _configSettings.YouTubeApiKey, "date");
            response = await _webHelperService.HttpGetAsync(url);
            dynamic parsed2 = JObject.Parse(response);
            var videos = parsed2.items;
            
            foreach (var video in videos)
            {
                var snippet = video.snippet;

                if (snippet != null)
                {
                    return new Video
                    {
                        Title = snippet.title,
                        Url = string.Format(UrlFormat, snippet.resourceId.videoId),
                        Published = snippet.publishedAt
                    };
                }
            }

            return null;
        }
    }
}
