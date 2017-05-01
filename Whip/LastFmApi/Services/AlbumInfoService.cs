using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Album;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class AlbumInfoService : IAlbumInfoService
    {
        public AlbumInfoService()
        {

        }

        public async Task<string> GetArtworkUrl(ApiClient client, string artistName, string albumTitle)
        {
            var getInfoMethod = new GetInfoMethod(client, artistName, albumTitle);
            var albumInfo = await getInfoMethod.GetResultAsync();
            return albumInfo.ArtworkUrl;
        }
    }
}
