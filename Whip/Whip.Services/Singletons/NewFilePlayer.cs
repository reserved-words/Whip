using System;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using static Whip.Common.Resources;

namespace Whip.Services.Singletons
{
    public class NewFilePlayer : IPlayer
    {
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IPlayer _basePlayer;
        private readonly IFileService _fileService;

        private string _currentlyPlayingFilepath;
        
        public NewFilePlayer(ICurrentDateTime currentDateTime, IPlayer basePlayer, IFileService fileService)
        {
            _currentDateTime = currentDateTime;
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
            _currentlyPlayingFilepath = _fileService.CopyFile(track.File.FullPath, CurrentPlayingDirectoryName, $"copy_{_currentDateTime.Get().Ticks}");
        }

        private void DeletePlayedFiles()
        {
            var fileToKeep = _currentlyPlayingFilepath != null
                ? System.IO.Path.GetFileName(_currentlyPlayingFilepath)
                : "";

            _fileService.DeleteFiles(CurrentPlayingDirectoryName, fileToKeep);
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

        public void Stop()
        {
            _basePlayer.Stop();
        }
    }
}
