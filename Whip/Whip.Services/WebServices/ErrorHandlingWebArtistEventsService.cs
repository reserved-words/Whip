using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class ErrorHandlingWebArtistEventsService : IWebArtistEventsService
    {
        private readonly IWebArtistEventsService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingWebArtistEventsService(IWebArtistEventsService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<List<ArtistEvent>> GetEventsAsync(Artist artist)
        {
            return await _interceptor.TryMethod(
                _service.GetEventsAsync(artist),
                new List<ArtistEvent>(),
                "GetEventsAsync(ArtistName: " + artist.Name + ")");
        }
    }
}
