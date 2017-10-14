using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface ILyricsService
    {
        Task<string> GetLyrics(string artistName, string trackTitle);
    }
}
