using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class PlayerControlsViewModelTests
    {
        private Mock<Library> _mockLibrary;
        private Mock<IPlaylist> _mockPlaylist;
        private Mock<IPlayer> _mockPlayer;
        private Mock<IPlayRequestHandler> _mockPlayRequestHandler;
        private Mock<TrackTimer> _mockTrackTimer;

        private PlayerControlsViewModel GetSubjectUnderTest()
        {
            _mockLibrary = new Mock<Library>();
            _mockPlaylist = new Mock<IPlaylist>();
            _mockPlayer = new Mock<IPlayer>();
            _mockPlayRequestHandler = new Mock<IPlayRequestHandler>();
            _mockTrackTimer = new Mock<TrackTimer>();

            _mockLibrary.Object.Artists = new List<Artist>
            {
                new Artist {Name = "Artist1", Grouping = "Grouping 1"},
                new Artist {Name = "Artist2", Grouping = null},
                new Artist {Name = "Artist3", Grouping = "Grouping 2"},
                new Artist {Name = "Artist4", Grouping = "Grouping 1"},
                new Artist {Name = "Artist5", Grouping = ""},
                new Artist {Name = "Artist6", Grouping = "Grouping 3"}
            };

            return new PlayerControlsViewModel(_mockLibrary.Object, _mockPlaylist.Object,
                _mockPlayer.Object, _mockPlayRequestHandler.Object, _mockTrackTimer.Object);
        }
        
        [TestMethod]
        public void Groupings_AreUpdatedOnLibraryUpdate()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.Groupings = new List<string>();

            // Act
            _mockLibrary.Raise(l => l.Updated += null, It.IsAny<Track>());

            // Assert
            sut.Groupings.Should().BeEquivalentTo("Grouping 1", "Grouping 2", "Grouping 3");
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
        public void OnCurrentTrackChanged_GivenNullTrack_DoesNotPlayNewTrack()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.OnCurrentTrackChanged(null);

            // Assert
            _mockPlayer.Verify(p => p.Play(It.IsAny<Track>()), Times.Never);
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

        [TestMethod]
        public void PlayGroupingCommand_SendsPlayRequest()
        {
            // Arrange
            var testGrouping = "some grouping";
            var sut = GetSubjectUnderTest();

            // Act
            sut.PlayGroupingCommand.Execute(testGrouping);

            // Assert
            _mockPlayRequestHandler.Verify(p => p.PlayGrouping(testGrouping, SortType.Random, null), Times.Once);
        }

        [TestMethod]
        public void ShuffleLibraryCommand_SendsPlayRequest()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.ShuffleLibraryCommand.Execute(0);

            // Assert
            _mockPlayRequestHandler.Verify(p => p.PlayAll(SortType.Random, null), Times.Once);
        }
    }
}
