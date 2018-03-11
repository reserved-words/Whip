﻿using System;
using System.Threading;
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

        private int _volume = 50;

        public Player(ILoggingService logger)
        {
            _logger = logger;
            _player.settings.volume = _volume;
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

            try
            {
                // This is to avoid a weird error where tracks don't start playing unless there's some delay here
                // - obviously this should not be the permanent solution
                Thread.Sleep(500);
                
                if (string.IsNullOrEmpty(track.File?.FullPath) || !System.IO.File.Exists(track.File.FullPath))
                    throw new ApplicationException("The requested file does not exist");

                _player.URL = track.File.FullPath;
                _player.controls.currentPosition = 0;
                _player.controls.play();
            }
            catch (Exception ex)
            {
                _logger.Error("Error playing file " + (track.File?.FullPath ?? "") + ": " + ex.Message);
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

        public int GetVolumePercentage()
        {
            return _volume;
        }

        public void SetVolumePercentage(int percentage)
        {
            _volume = percentage < 0
                ? 0
                : percentage > 100
                ? 100
                : percentage;

            _player.settings.volume = _volume;
        }

        public void Mute()
        {
            _player.settings.volume = 0;
        }

        public void Unmute()
        {
            _player.settings.volume = _volume;
        }
    }
}
