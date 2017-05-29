using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IWebArtistEventsService
    {
        Task<List<ArtistEvent>> GetEventsAsync(Artist artist);
    }
}
