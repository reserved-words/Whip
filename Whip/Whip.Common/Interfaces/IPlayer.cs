using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Common.Interfaces
{
    public interface IPlayer
    {
        int GetVolumePercentage();
        void Mute();
        void Play(Track track);
        void Pause();
        void Resume();
        void SetVolumePercentage(int volume);
        void SkipToPercentage(double newPercentage);
        void Unmute();
        void Stop();
    }
}
