using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingArtistInfoService : IArtistInfoService
    {
        private readonly IArtistInfoService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingArtistInfoService(IArtistInfoService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<bool> PopulateArtistInfo(Artist artist, int numberOfSimilarArtists)
        {
            return await _interceptor.TryMethod(
                _service.PopulateArtistInfo(artist, numberOfSimilarArtists), 
                false,
                WebServiceType.LastFm,
                "Populate Artist Images (Artist: " + artist.Name + ")");
        }
    }
}
