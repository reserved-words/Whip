using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Interfaces;
using Whip.Common.Model;

namespace Whip.Services.Singletons
{
    public class NewFilePlayer : IPlayer
    {
        private const string CurrentPlayingDirectoryName = "CurrentlyPlaying";

        private readonly IPlayer _basePlayer;

        private readonly DirectoryInfo _currentlyPlayingDirectory;
        private readonly string _currentlyPlayingDirectoryPath;

        private string _currentlyPlayingFilepath;
        
        public NewFilePlayer(IPlayer basePlayer)
        {
            _basePlayer = basePlayer;

            _currentlyPlayingDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CurrentPlayingDirectoryName);
            _currentlyPlayingDirectory = Directory.CreateDirectory(_currentlyPlayingDirectoryPath);
        }

        public void Pause()
        {
            _basePlayer.Pause();
        }

        public void Play(Track track)
        {
            if (track == null)
            {
                _basePlayer.Play(track);
            }
            else
            {
                CopyFile(track);

                _basePlayer.Play(new Track { File = new Common.Model.File(_currentlyPlayingFilepath, null, DateTime.MinValue, DateTime.MinValue) });
            }

            Task.Run(() => DeletePlayedFiles());
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
            var copyFilename = string.Format("playing_{0}{1}", DateTime.Now.Ticks, Path.GetExtension(track.File.FullPath));

            _currentlyPlayingFilepath = Path.Combine(_currentlyPlayingDirectoryPath, copyFilename);

            System.IO.File.Copy(track.File.FullPath, _currentlyPlayingFilepath);
            System.IO.File.SetAttributes(_currentlyPlayingFilepath, FileAttributes.Normal);
        }

        private void DeletePlayedFiles()
        {
            _currentlyPlayingDirectory
                .EnumerateFiles()
                .Where(f => _currentlyPlayingFilepath == null || f.Name != _currentlyPlayingFilepath)
                .ToList()
                .ForEach(f =>
                {
                    f.Attributes = FileAttributes.Normal;
                    f.Delete();
                });
        }
    }
}
