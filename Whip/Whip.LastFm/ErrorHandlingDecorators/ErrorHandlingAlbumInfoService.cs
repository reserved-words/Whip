using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingAlbumInfoService : IAlbumInfoService
    {
        private readonly IAlbumInfoService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingAlbumInfoService(IAlbumInfoService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<string> GetArtworkUrl(string artistName, string albumTitle)
        {
            return await _interceptor.TryMethod(
                _service.GetArtworkUrl(artistName, albumTitle),
                string.Empty,
                WebServiceType.LastFm,
                "Get Artwork URL (Artist: " + artistName + ", Title: " + albumTitle + ")");
        }
    }
}
