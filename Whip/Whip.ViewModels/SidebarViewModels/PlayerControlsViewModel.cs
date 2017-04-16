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

namespace Whip.ViewModels
{
    public class PlayerControlsViewModel : ViewModelBase
    {
        private enum PlayerStatus { Playing, Paused, Stopped }

        private readonly Library _library;
        private readonly IMessenger _messenger;

        private List<string> _groupings;
        private PlayerStatus _currentStatus;
        
        public PlayerControlsViewModel(Library library, IMessenger messenger) 
        {
            _library = library;
            _messenger = messenger;

            _library.Updated += OnLibraryUpdated;
            
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
        
        private void OnLibraryUpdated()
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
            _messenger.Send(new PlayNextMessage());
        }

        private void OnMovePrevious()
        {
            _messenger.Send(new PlayPreviousMessage());
        } 

        private void OnPause()
        {
            _messenger.Send(new PausePlayerMessage());
            TrackTimer.Stop();
            CurrentStatus = PlayerStatus.Paused;
        }

        private void OnResume()
        {
            _messenger.Send(new ResumePlayerMessage());
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
            _messenger.Send(new SkipToPercentageMessage(newPercentage));
            TrackTimer.SkipToPercentage(newPercentage);
        }

        private void OnTrackEnded()
        {
            _messenger.Send(new PlayNextMessage());
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
