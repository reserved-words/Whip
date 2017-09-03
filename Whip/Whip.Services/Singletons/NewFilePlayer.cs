using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Singletons
{
    public class NewFilePlayer : IPlayer
    {
        private const string ApplicationDirectoryName = "Whip";
        private const string CurrentPlayingDirectoryName = "CurrentlyPlaying";

        private readonly IPlayer _basePlayer;

        private readonly DirectoryInfo _currentlyPlayingDirectory;
        private readonly string _currentlyPlayingDirectoryPath;
        private readonly ILoggingService _logger;

        private string _currentlyPlayingFilepath;
        
        public NewFilePlayer(IPlayer basePlayer, ILoggingService logger)
        {
            _basePlayer = basePlayer;
            _logger = logger;

            _currentlyPlayingDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationDirectoryName, CurrentPlayingDirectoryName);
            _currentlyPlayingDirectory = Directory.CreateDirectory(_currentlyPlayingDirectoryPath);
        }

        public void Pause()
        {
            _basePlayer.Pause();
        }

        public void Play(Track track)
        {
            _logger.Info("NewFilePlayer: Play " + track.Title);

            if (track == null)
            {
                _basePlayer.Play(track);
            }
            else
            {
                CopyFile(track);

                _logger.Info("Invoke basePlayer.Play");
                _basePlayer.Play(new Track { File = new Common.Model.File(_currentlyPlayingFilepath, null, DateTime.MinValue, DateTime.MinValue) });
            }

            _logger.Info("Deleting played files");
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

            _logger.Info("Copying file " + track.File.FullPath + " to " + _currentlyPlayingFilepath);

            System.IO.File.Copy(track.File.FullPath, _currentlyPlayingFilepath);
            System.IO.File.SetAttributes(_currentlyPlayingFilepath, FileAttributes.Normal);

            _logger.Info(System.IO.File.Exists(_currentlyPlayingFilepath)
                ? "File exists!"
                : "File does not exist!");
        }

        private void DeletePlayedFiles()
        {
            _logger.Info("Deleting all files except " + _currentlyPlayingFilepath ?? "");

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
