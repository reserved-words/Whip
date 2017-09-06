using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
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
    }
}
