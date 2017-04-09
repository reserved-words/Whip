using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.Common.TrackSorters;
using System.Collections.Generic;
using Whip.Common.ExtensionMethods;

namespace Whip.ViewModels
{
    public class PlayerControlsViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly Playlist _playlist;
        private readonly ITrackFilterService _trackFilterService;

        private Track _currentTrack;
        private Library _library;
        private List<string> _groupings;
        private bool _playing;
        
        public PlayerControlsViewModel(Library library, Playlist playlist, ITrackFilterService trackFilterService, IMessenger messenger)
        {
            _library = library;
            _messenger = messenger;
            _playlist = playlist;
            _trackFilterService = trackFilterService;

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

            Playing = false;
        }

        public Track CurrentTrack
        {
            get { return _currentTrack; }
            set { Set(ref _currentTrack, value); }
        }

        public List<string> Groupings
        {
            get { return _groupings; }
            set { Set(ref _groupings, value); }
        }

        public bool Playing
        {
            get { return _playing; }
            set
            {
                Set(ref _playing, value);
                PauseCommand.RaiseCanExecuteChanged();
                ResumeCommand.RaiseCanExecuteChanged();
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

        private bool CanPause() => Playing;

        private bool CanResume() => _playlist.Any() && !Playing;

        private void OnLibraryUpdated()
        {
            Groupings = _library.GetGroupings();
        }

        private void OnPlaylistUpdated()
        {
            MoveNextCommand.RaiseCanExecuteChanged();
            MovePreviousCommand.RaiseCanExecuteChanged();
            PauseCommand.RaiseCanExecuteChanged();
            ResumeCommand.RaiseCanExecuteChanged();
        }

        public void OnCurrentTrackChanged(Track track)
        {
            CurrentTrack = track;
            SetTrackTimer(track);
        }

        private bool CanMovePrevious() => _playlist.Any();

        private bool CanMoveNext() => _playlist.Any();

        private bool CanSkip(double newPercentage) => CurrentTrack != null;

        private void OnMoveNext() => _playlist.MoveNext();

        private void OnMovePrevious() => _playlist.MovePrevious();

        private void OnPause()
        {
            _messenger.Send(new PausePlayerMessage());
            TrackTimer.Stop();
            Playing = false;
        }

        private void OnResume()
        {
            _messenger.Send(new ResumePlayerMessage());
            TrackTimer.Start();
            Playing = true;
        }

        private void OnShuffleGrouping(string grouping)
        {
            _playlist.Set(_trackFilterService.GetAll(new RandomTrackSorter(), grouping));
            Playing = true;
        }

        private void OnShuffleLibrary()
        {
            _playlist.Set(_trackFilterService.GetAll(new RandomTrackSorter()));
            Playing = true;
        }

        private void OnSkip(double newPercentage)
        {
            _messenger.Send(new SkipToPercentageMessage(newPercentage));
            TrackTimer.SkipToPercentage(newPercentage);
        }

        private void OnTrackEnded()
        {
            _playlist.MoveNext();
        }

        private void SetTrackTimer(Track track)
        {
            TrackTimer.Reset(track);
            TrackTimer.Start();
        }
    }
}
