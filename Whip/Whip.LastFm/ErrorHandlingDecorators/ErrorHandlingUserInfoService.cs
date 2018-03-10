using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingUserInfoService : IRecentTracksService
    {
        private readonly IRecentTracksService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingUserInfoService(IRecentTracksService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<ICollection<TrackPlay>> GetRecentTrackPlays(string username, int limit)
        {
            return await _interceptor.TryMethod(
                _service.GetRecentTrackPlays(username, limit),
                new List<TrackPlay>(),
                WebServiceType.LastFm,
                "Get Recent Tracks (Username: " + username + ", Limit: " + limit + ")");
        }
    }
}
