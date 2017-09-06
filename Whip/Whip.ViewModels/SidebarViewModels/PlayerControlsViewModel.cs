using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common;
using Whip.ViewModels.Utilities;
using System.Collections.Generic;
using Whip.Common.ExtensionMethods;
using System.Linq;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels
{
    public class PlayerControlsViewModel : ViewModelBase
    {
        private enum PlayerStatus { Playing, Paused, Stopped }

        private readonly Library _library;
        private readonly IPlaylist _playlist;
        private readonly IPlayer _player;
        private readonly IPlayRequestHandler _playRequestHandler;

        private List<string> _groupings;
        private PlayerStatus _currentStatus;
        
        public PlayerControlsViewModel(Library library, IPlaylist playlist, IPlayer player,
            IPlayRequestHandler playRequestHandler)
        {
            _library = library;
            _playlist = playlist;
            _player = player;
            _playRequestHandler = playRequestHandler;

            _library.Updated += OnLibraryUpdated;
            _playlist.ListUpdated += OnPlaylistUpdated;

            MoveNextCommand = new RelayCommand(OnMoveNext, CanMoveNext);
            MovePreviousCommand = new RelayCommand(OnMovePrevious, CanMovePrevious);
            PauseCommand = new RelayCommand(OnPause, CanPause);
            PlayGroupingCommand = new RelayCommand<string>(OnShuffleGrouping);
            ResumeCommand = new RelayCommand(OnResume, CanResume);
            ShuffleLibraryCommand = new RelayCommand(OnShuffleLibrary);
            SkipToPercentageCommand = new RelayCommand<double>(OnSkip, CanSkip);

            TrackTimer = new TrackTimer();
            TrackTimer.TrackEnded += OnTrackEnded;

            CurrentStatus = PlayerStatus.Stopped;
        }

        private void OnPlaylistUpdated()
        {
            if (_playlist.CurrentTrack == null)
            {
                _playlist.MoveNext();
            }
        }

        public bool Playing => CurrentStatus == PlayerStatus.Playing;
        
        public List<string> Groupings
        {
            get { return _groupings; }
            set { Set(ref _groupings, value); }
        }

        private PlayerStatus CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                Set(ref _currentStatus, value);
                RaisePlayerStatusChanged();
            }
        }

        public TrackTimer TrackTimer { get; }
        public RelayCommand MoveNextCommand { get; }
        public RelayCommand MovePreviousCommand { get; }
        public RelayCommand PauseCommand { get; }
        public RelayCommand<string> PlayGroupingCommand { get; }
        public RelayCommand ResumeCommand { get; }
        public RelayCommand ShuffleLibraryCommand { get; }
        public RelayCommand<double> SkipToPercentageCommand { get; }

        private bool CanMovePrevious() => CurrentStatus != PlayerStatus.Stopped;
        private bool CanMoveNext() => CurrentStatus != PlayerStatus.Stopped;
        private bool CanPause() => CurrentStatus == PlayerStatus.Playing;
        private bool CanResume() => CurrentStatus == PlayerStatus.Paused;
        private bool CanSkip(double newPercentage) => CurrentStatus != PlayerStatus.Stopped;
        
        private void OnLibraryUpdated(Track track)
        {
            Groupings = _library.GetGroupings().Where(g => !string.IsNullOrEmpty(g)).ToList();
        }

        public void OnCurrentTrackChanged(Track track)
        {
            CurrentStatus = track == null
                ? PlayerStatus.Stopped
                : PlayerStatus.Playing;

            _player.Play(track);

            TrackTimer.Reset(track);
            TrackTimer.Start();
        }

        private void OnMoveNext()
        {
            _playlist.MoveNext();
        }

        private void OnMovePrevious()
        {
            _playlist.MovePrevious();
        } 

        private void OnPause()
        {
            _player.Pause();
            TrackTimer.Stop();
            CurrentStatus = PlayerStatus.Paused;
        }

        private void OnResume()
        {
            _player.Resume();
            TrackTimer.Start();
            CurrentStatus = PlayerStatus.Playing;
        }

        private void OnShuffleGrouping(string grouping)
        {
            _playRequestHandler.PlayGrouping(grouping, SortType.Random);
        }

        private void OnShuffleLibrary()
        {
            _playRequestHandler.PlayAll(SortType.Random);
        }

        private void OnSkip(double newPercentage)
        {
            _player.SkipToPercentage(newPercentage);
            TrackTimer.SkipToPercentage(newPercentage);
        }

        private void OnTrackEnded()
        {
            _playlist.MoveNext();
        }

        private void RaisePlayerStatusChanged()
        {
            MoveNextCommand.RaiseCanExecuteChanged();
            MovePreviousCommand.RaiseCanExecuteChanged();
            PauseCommand.RaiseCanExecuteChanged();
            ResumeCommand.RaiseCanExecuteChanged();
            SkipToPercentageCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(Playing));
        }
    }
}
