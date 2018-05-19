using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.TabViewModels;
using Whip.ViewModels.TabViewModels.Library;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class LibraryTracksViewModelTest
    {
        private Mock<TrackContextMenuViewModel> _mockTrackContextMenu;
        private Mock<IPlayRequestHandler> _mockPlayRequestHandler;
        private Mock<ILibrarySortingService> _mockSortingService;

        private readonly Artist _testArtist = new Artist { Name = "Artist Name" };
        private readonly Track _testTrack = new Track();
        private readonly IEnumerable<Track> _testArtistTracks = new List<Track> { new Track { Title = "Track 1" } };
        private readonly IEnumerable<Track> _testAlbumTracks = new List<Track> { new Track { Title = "Track 2" } };
        
        private LibraryTracksViewModel GetSubjectUnderTest()
        {
            _mockTrackContextMenu = new Mock<TrackContextMenuViewModel>();
            _mockPlayRequestHandler = new Mock<IPlayRequestHandler>();
            _mockSortingService = new Mock<ILibrarySortingService>();

            _mockSortingService
                .Setup(s => s.GetArtistTracksInDefaultOrder(_testArtist))
                .Returns(_testArtistTracks);

            _mockSortingService
                .Setup(s => s.GetAlbumTracksInDefaultOrder(_testArtist))
                .Returns(_testAlbumTracks);

            return new LibraryTracksViewModel(
                _mockTrackContextMenu.Object,
                _mockPlayRequestHandler.Object,
                _mockSortingService.Object);
        }

        [TestMethod]
        public void PlayAlbumCommand_OnExecute_PlaysSelectedAlbum()
        {
            // Arrange
            var testAlbum = new Album();
            var sut = GetSubjectUnderTest();
            sut.SelectedAlbum = testAlbum;

            // Act
            sut.PlayAlbumCommand.Execute(null);

            // Assert
            _mockPlayRequestHandler.Verify(h => h.PlayAlbum(testAlbum, SortType.Ordered, null), Times.Once);
        }

        [TestMethod]
        public void PlayArtistCommand_OnExecute_PlaysAllArtistTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Artist = _testArtist;
            sut.SelectedTrack = _testTrack;
            sut.PlayArtistCommand.Execute(null);

            // Assert
            _mockPlayRequestHandler.Verify(h => h.PlayArtist(_testArtist, SortType.Random, _testTrack), Times.Once);
        }

        [TestMethod]
        public void OnArtistChanged_UpdatesTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Artist = _testArtist;

            // Assert
            sut.Tracks.Should().BeEquivalentTo(_testAlbumTracks);
        }

        [TestMethod]
        public void OnSelectedTrackChanged_UpdatesContextMenu()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.SelectedTrack = _testTrack;

            // Assert
            _mockTrackContextMenu.Verify(t => t.SetTrack(_testTrack), Times.Once);
        }

        [TestMethod]
        public void UpdateTracks_GivenDisplayTracksByArtist_UpdatesTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Artist = _testArtist;
            sut.UpdateTracks(true);

            // Assert
            sut.Tracks.Should().BeEquivalentTo(_testArtistTracks);
        }

        [TestMethod]
        public void UpdateTracks_GivenDisplayTracksByAlbum_UpdatesTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Artist = _testArtist;
            sut.UpdateTracks(false);

            // Assert
            sut.Tracks.Should().BeEquivalentTo(_testAlbumTracks);
        }
    }
}
