using System;
using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class ArtistWebInfoService : IArtistWebInfoService
    {
        private readonly IConfigSettings _configSettings;
        private readonly IVideoService _videoService;
        private readonly IEventsService _eventsService;
        private readonly IArtistInfoService _artistInfoService;
        private readonly ITwitterService _twitterService;
        private readonly IAsyncMethodInterceptor _interceptor;
        
        public ArtistWebInfoService(IVideoService videoService, IEventsService eventsService, IArtistInfoService artistInfoService,
            ITwitterService twitterService, IConfigSettings configSettings, IAsyncMethodInterceptor interceptor)
        {
            _configSettings = configSettings;
            _videoService = videoService;
            _eventsService = eventsService;
            _artistInfoService = artistInfoService;
            _twitterService = twitterService;
            _interceptor = interceptor;
        }

        public async Task<bool> PopulateEventsAsync(Artist artist)
        {
            if (artist == null)
                return true;

            if (artist.UpcomingEventsUpdated.AddDays(_configSettings.DaysBeforeUpdatingArtistWebInfo) > DateTime.Now)
                return true;
            
            var success = await _interceptor.TryMethod<bool>(
                _eventsService.PopulateEventsAsync(artist),
                false,
                WebServiceType.LastFm,
                GetMethodDescription(nameof(PopulateEventsAsync), artist));

            if (success)
            {
                artist.UpcomingEventsUpdated = DateTime.Now;
            }

            return success;
        }

        public async Task<bool> PopulateLatestVideoAsync(Artist artist)
        {
            if (artist == null)
                return true;

            if (artist.VideoUpdated.AddDays(_configSettings.DaysBeforeUpdatingArtistWebInfo) > DateTime.Now)
                return true;
            
            var success = await _interceptor.TryMethod(
                _videoService.PopulateLatestVideoAsync(artist),
                false,
                WebServiceType.LastFm,
                GetMethodDescription(nameof(PopulateLatestVideoAsync), artist));

            if (success)
            {
                artist.VideoUpdated = DateTime.Now;
            }

            return success;
        }

        public async Task<bool> PopulateArtistInfo(Artist artist, int numberOfSimilarArtists)
        {
            if (artist == null)
                return true;

            if (artist.WebInfo.Updated.AddDays(_configSettings.DaysBeforeUpdatingArtistWebInfo) > DateTime.Now)
                return true;
            
            var success = await _interceptor.TryMethod(
                _artistInfoService.PopulateArtistInfo(artist, numberOfSimilarArtists),
                false,
                WebServiceType.LastFm,
                GetMethodDescription(nameof(PopulateArtistInfo), artist));

            if (success)
            {
                artist.WebInfo.Updated = DateTime.Now;
            }

            return success;
        }

        public async Task<bool> PopulateTweets(Artist artist)
        {
            if (artist == null)
                return true;

            if (artist.TweetsUpdated.AddMinutes(_configSettings.MinutesBeforeUpdatingTweets) > DateTime.Now)
                return true;

            var success = await _interceptor.TryMethod(
                _twitterService.PopulateTweets(artist),
                false,
                WebServiceType.LastFm,
                GetMethodDescription(nameof(PopulateTweets), artist));

            if (success)
            {
                artist.TweetsUpdated = DateTime.Now;
            }

            return success;
        }

        private string GetMethodDescription(string methodName, Artist artist)
        {
            return string.Format("{0} (ArtistName: {1})", methodName, artist.Name);
        }
    }
}
