using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IEventsService
    {
        Task<bool> PopulateEventsAsync(Artist artist);
    }
}
