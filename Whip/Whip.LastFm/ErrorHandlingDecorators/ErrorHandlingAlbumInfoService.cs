using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingAlbumInfoService : IWebAlbumInfoService
    {
        private readonly IWebAlbumInfoService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingAlbumInfoService(IWebAlbumInfoService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<string> GetArtworkUrl(string artistName, string albumTitle)
        {
            return await _interceptor.TryMethod(
                _service.GetArtworkUrl(artistName, albumTitle),
                string.Empty,
                "Get Artwork URL (Artist: " + artistName + ", Title: " + albumTitle + ")");
        }
    }
}
