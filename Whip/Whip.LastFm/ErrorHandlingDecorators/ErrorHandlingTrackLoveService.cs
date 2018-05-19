using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingTrackLoveService : ITrackLoveService
    {
        private readonly ITrackLoveService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingTrackLoveService(ITrackLoveService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }
        
        public async Task<bool> IsLovedAsync(Track track)
        {
            return await _interceptor.TryMethod(
                _service.IsLovedAsync(track),
                false,
                WebServiceType.LastFm,
                "IsLoved(Track: " + track.Title + " by " + track.Artist.Name + ")");
        }

        public async Task<bool> LoveTrackAsync(Track track)
        {
            return await _interceptor.TryMethod(
                _service.LoveTrackAsync(track),
                false,
                WebServiceType.LastFm,
                "LoveTrackAsync(Track: " + track.Title + " by " + track.Artist.Name + ")");
        }

        public async Task<bool> UnloveTrackAsync(Track track)
        {
            return await _interceptor.TryMethod(
                _service.UnloveTrackAsync(track),
                false,
                WebServiceType.LastFm,
                "UnloveTrackAsync(Track: " + track.Title + " by " + track.Artist.Name + ")");
        }
    }
}
