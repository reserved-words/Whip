using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Interfaces
{
    public interface IPlayerVolume
    {
        int GetVolumePercentage();
        void Mute();
        void SetVolumePercentage(int volume);
        void Unmute();
    }
}
