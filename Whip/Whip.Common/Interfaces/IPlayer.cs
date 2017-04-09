using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Interfaces
{
    public interface IPlayer
    {
        void Play(string filepath);
        void Pause();
        void Resume();
    }
}
