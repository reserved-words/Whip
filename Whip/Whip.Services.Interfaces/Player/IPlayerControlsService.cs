using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces.Player
{
    public interface IPlayerControlsService
    {
        void Play(string filepath);
        void Pause();
        void Resume();
    }
}
