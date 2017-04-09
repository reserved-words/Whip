using Whip.Common.Interfaces;
using Whip.Common.Model;
using WMPLib;

namespace Whip.WmpPlayer
{
    public class Player : IPlayer
    {
        private readonly WindowsMediaPlayer _player = new WindowsMediaPlayer();

        public Player()
        {
            _player.settings.volume = 50;
        }

        public void Pause()
        {
            _player.controls.pause();
        }

        public void Play(Track track)
        {
            if (track == null)
            {
                _player.URL = string.Empty;
                _player.controls.currentPosition = 0;
                _player.controls.stop();
                return;
            }

            _player.URL = track.FullFilepath;
            _player.controls.currentPosition = 0;
            _player.controls.play();
        }

        public void Resume()
        {
            _player.controls.play();
        }

        public void SkipToPercentage(double newPercentage)
        {
            if (_player.controls.currentItem == null)
                return;

            _player.controls.currentPosition = (newPercentage / 100) * _player.controls.currentItem.duration;
        }
    }
}
