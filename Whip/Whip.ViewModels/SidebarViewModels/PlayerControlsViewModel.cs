using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels
{
    public class PlayerControlsViewModel : ViewModelBase
    {
        public enum PlayerStatus { Playing, Paused, Stopped }

        private readonly IPlaylist _playlist;
        private readonly IPlayer _player;
        
        private PlayerStatus _currentStatus;
        
        public PlayerControlsViewModel(IPlaylist playlist, IPlayer player, TrackTimer trackTimer)
        {
            _playlist = playlist;
            _player = player;

            MoveNextCommand = new RelayCommand(OnMoveNext, CanMoveNext);
            MovePreviousCommand = new RelayCommand(OnMovePrevious, CanMovePrevious);
            PauseCommand = new RelayCommand(OnPause, CanPause);
            ResumeCommand = new RelayCommand(OnResume, CanResume);
            SkipToPercentageCommand = new RelayCommand<double>(OnSkip, CanSkip);

            TrackTimer = trackTimer;
            TrackTimer.TrackEnded += OnTrackEnded;

            CurrentStatus = PlayerStatus.Stopped;
        }

        public bool Playing => CurrentStatus == PlayerStatus.Playing;
        
        public PlayerStatus CurrentStatus
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
        public RelayCommand ResumeCommand { get; }
        public RelayCommand<double> SkipToPercentageCommand { get; }

        private bool CanMovePrevious() => CurrentStatus != PlayerStatus.Stopped;
        private bool CanMoveNext() => CurrentStatus != PlayerStatus.Stopped;
        private bool CanPause() => CurrentStatus == PlayerStatus.Playing;
        private bool CanResume() => CurrentStatus == PlayerStatus.Paused;
        private bool CanSkip(double newPercentage) => CurrentStatus != PlayerStatus.Stopped;
        
        public void OnCurrentTrackChanged(Track track)
        {
            CurrentStatus = track == null
                ? PlayerStatus.Stopped
                : PlayerStatus.Playing;

            TrackTimer.Reset(track);

            if (track == null)
                return;

            _player.Play(track);
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
