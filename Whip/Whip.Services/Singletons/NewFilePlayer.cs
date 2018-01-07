using System;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using static Whip.Common.Resources;

namespace Whip.Services.Singletons
{
    public class NewFilePlayer : IPlayer
    {
        private readonly IPlayer _basePlayer;
        private readonly IFileService _fileService;

        private string _currentlyPlayingFilepath;
        
        public NewFilePlayer(IPlayer basePlayer, IFileService fileService)
        {
            _basePlayer = basePlayer;
            _fileService = fileService;
        }

        public void Pause()
        {
            _basePlayer.Pause();
        }

        public void Play(Track track)
        {
            if (track == null)
            {
                _basePlayer.Play(null);
            }
            else
            {
                CopyFile(track);

                _basePlayer.Play(new Track { File = new File(_currentlyPlayingFilepath, null, DateTime.MinValue, DateTime.MinValue) });
            }

            DeletePlayedFiles();
        }

        public void Resume()
        {
            _basePlayer.Resume();
        }

        public void SkipToPercentage(double newPercentage)
        {
            _basePlayer.SkipToPercentage(newPercentage);
        }

        private void CopyFile(Track track)
        {
            _currentlyPlayingFilepath = _fileService.CopyFile(track.File.FullPath, CurrentPlayingDirectoryName);
        }

        private void DeletePlayedFiles()
        {
            _fileService.DeleteFiles(CurrentPlayingDirectoryName, _currentlyPlayingFilepath ?? "");
        }

        public int GetVolumePercentage()
        {
            return _basePlayer.GetVolumePercentage();
        }

        public void Mute()
        {
            _basePlayer.Mute();
        }

        public void SetVolumePercentage(int volume)
        {
            _basePlayer.SetVolumePercentage(volume);
        }

        public void Unmute()
        {
            _basePlayer.Unmute();
        }
    }
}
