using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.Sidebar.Tests
{
    [TestClass]
    public class PlayerControlsViewModelTests
    {
        private Mock<IPlaylist> _mockPlaylist;
        private Mock<IPlayer> _mockPlayer;
        private Mock<TrackTimer> _mockTrackTimer;

        private PlayerControlsViewModel GetSubjectUnderTest()
        {
            _mockPlaylist = new Mock<IPlaylist>();
            _mockPlayer = new Mock<IPlayer>();
            _mockTrackTimer = new Mock<TrackTimer>();

            return new PlayerControlsViewModel(_mockPlaylist.Object, _mockPlayer.Object, _mockTrackTimer.Object);
        }
        
        [TestMethod]
        public void OnCurrentTrackChanged_GivenNullTrack_UpdatesPlayingStatus()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(null);

            // Assert
            sut.Playing.Should().BeFalse();
        }

        [TestMethod]
        public void OnCurrentTrackChanged_GivenNotNullTrack_UpdatesPlayingStatus()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(new Track());

            // Assert
            sut.Playing.Should().BeTrue();
        }

        [TestMethod]
        public void OnCurrentTrackChanged_GivenNullTrack_PlaysNewTrack()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(null);

            // Assert
            _mockPlayer.Verify(p => p.Play(It.IsAny<Track>()), Times.Once);
        }

        [TestMethod]
        public void OnCurrentTrackChanged_GivenNotNullTrack_PlaysNewTrack()
        {
            // Arrange
            var testTrack = new Track();
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(testTrack);

            // Assert
            _mockPlayer.Verify(p => p.Play(testTrack), Times.Once);
        }

        [TestMethod]
        public void OnCurrentTrackChanged_GivenNullTrack_ResetsTrackTimer()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(null);

            // Assert
            _mockTrackTimer.Verify(t => t.Reset(null), Times.Once);
            _mockTrackTimer.Verify(t => t.Start(), Times.Never);
        }

        [TestMethod]
        public void OnCurrentTrackChanged_GivenNotNullTrack_ResetsAndStartsTrackTimer()
        {
            // Arrange
            var testTrack = new Track();
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(testTrack);

            // Assert
            _mockTrackTimer.Verify(t => t.Reset(testTrack), Times.Once);
            _mockTrackTimer.Verify(t => t.Start(), Times.Once);
        }

        [TestMethod]
        public void MoveNextCommand_GivenPlaying_CallsPlaylistMoveNext()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.CurrentStatus = PlayerControlsViewModel.PlayerStatus.Playing;

            // Act
            sut.MoveNextCommand.Execute(0);

            // Assert
            _mockPlaylist.Verify(p => p.MoveNext(), Times.Once);
        }

        [TestMethod]
        public void MovePreviousCommand_GivenPlaying_CallsPlaylistMovePrevious()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.CurrentStatus = PlayerControlsViewModel.PlayerStatus.Playing;

            // Act
            sut.MovePreviousCommand.Execute(0);

            // Assert
            _mockPlaylist.Verify(p => p.MovePrevious(), Times.Once);
        }

        [TestMethod]
        public void PauseCommand_GivenPlaying_PausesPlay()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.CurrentStatus = PlayerControlsViewModel.PlayerStatus.Playing;

            // Act
            sut.PauseCommand.Execute(0);

            // Assert
            _mockPlayer.Verify(p => p.Pause(), Times.Once);
            _mockTrackTimer.Verify(t => t.Stop(), Times.Once);
            sut.CurrentStatus.Should().Be(PlayerControlsViewModel.PlayerStatus.Paused);
        }

        [TestMethod]
        public void ResumeCommand_GivenPaused_ResumesPlay()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.CurrentStatus = PlayerControlsViewModel.PlayerStatus.Paused;

            // Act
            sut.ResumeCommand.Execute(0);

            // Assert
            _mockPlayer.Verify(p => p.Resume(), Times.Once);
            _mockTrackTimer.Verify(t => t.Start(), Times.Once);
            sut.CurrentStatus.Should().Be(PlayerControlsViewModel.PlayerStatus.Playing);
        }
    }
}
