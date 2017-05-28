using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingArtistInfoService : IWebArtistInfoService
    {
        private readonly IWebArtistInfoService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingArtistInfoService(IWebArtistInfoService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<ArtistWebInfo> PopulateArtistInfo(Artist artist)
        {
            return await _interceptor.TryMethod(
                _service.PopulateArtistInfo(artist), 
                artist.WebInfo, 
                "Populate Artist Images (Artist: " + artist.Name + ")");
        }
    }
}
