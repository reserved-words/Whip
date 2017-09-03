using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using System.Collections.Generic;
using Whip.Common.ExtensionMethods;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels
{
    public class PlayerControlsViewModel : ViewModelBase
    {
        private enum PlayerStatus { Playing, Paused, Stopped }

        private readonly Library _library;
        private readonly IMessenger _messenger;
        private readonly IPlaylist _playlist;
        private readonly IPlayer _player;

        private List<string> _groupings;
        private PlayerStatus _currentStatus;
        
        public PlayerControlsViewModel(Library library, IMessenger messenger, IPlaylist playlist, IPlayer player) 
        {
            _library = library;
            _messenger = messenger;
            _playlist = playlist;
            _player = player;

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

        public TrackTimer TrackTimer { get; private set; }

        public RelayCommand MoveNextCommand { get; private set; }
        public RelayCommand MovePreviousCommand { get; private set; }
        public RelayCommand PauseCommand { get; private set; }
        public RelayCommand<string> PlayGroupingCommand { get; private set; }
        public RelayCommand ResumeCommand { get; private set; }
        public RelayCommand ShuffleLibraryCommand { get; private set; }
        public RelayCommand<double> SkipToPercentageCommand { get; private set; }

        private bool CanMovePrevious() => CurrentStatus != PlayerStatus.Stopped;

        private bool CanMoveNext() => CurrentStatus != PlayerStatus.Stopped;

        private bool CanPause() => CurrentStatus == PlayerStatus.Playing;

        private bool CanResume() => CurrentStatus == PlayerStatus.Paused;

        private bool CanSkip(double newPercentage) => CurrentStatus != PlayerStatus.Stopped;
        
        private void OnLibraryUpdated(Track track)
        {
            Groupings = _library.GetGroupings().Where(g => !string.IsNullOrEmpty(g)).ToList();
        }

        public void OnNewTrackStarted(Track track)
        {
            TrackTimer.Reset(track);
            TrackTimer.Start();
        }

        public void OnCurrentTrackChanged(Track track)
        {
            TrackTimer.Reset(null);
            TrackTimer.Stop();

            if (track == null)
            {
                CurrentStatus = PlayerStatus.Stopped;
            }
            else
            {
                CurrentStatus = PlayerStatus.Playing;
            }
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
            _messenger.Send(new PlayGroupingMessage(grouping, SortType.Random));
        }

        private void OnShuffleLibrary()
        {
            _messenger.Send(new PlayAllMessage(SortType.Random));
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
