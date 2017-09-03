using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using WMPLib;

namespace Whip.WmpPlayer
{
    public class Player : IPlayer
    {
        private readonly WindowsMediaPlayer _player = new WindowsMediaPlayer();
        private readonly ILoggingService _logger;

        public Player(ILoggingService logger)
        {
            _logger = logger;
            _player.settings.volume = 50;
        }
        
        public void Pause()
        {
            _player.controls.pause();
        }

        public void Play(Track track)
        {
            _logger.Info("Player: Play " + track.File.FullPath);

            if (track == null)
            {
                _logger.Error("Error playing file: Track is null");
                _player.URL = string.Empty;
                _player.controls.currentPosition = 0;
                _player.controls.stop();
                return;
            }

            try
            {
                _player.URL = track.File.FullPath;

                _logger.Info("Playing " + _player.URL);
                _logger.Info("Duration " + _player.controls.currentItem.duration);

                _player.controls.currentPosition = 0;
                _player.controls.play();
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error playing file: " + ex.Message);
            }
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
