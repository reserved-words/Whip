using LastFmApi.Interfaces;
using LastFmApi.Internal;
using LastFmApi.Methods.Artist;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class ArtistInfoService : IArtistInfoService
    {
        public async Task<ArtistInfo> GetInfo(ApiClient client, string artistName)
        {
            var getInfoMethod = new GetInfoMethod(client, artistName);
            return await getInfoMethod.GetResultAsync();
        }
    }
}
