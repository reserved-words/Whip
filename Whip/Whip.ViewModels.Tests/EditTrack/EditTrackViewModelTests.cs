using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels;
using Whip.Common.Singletons;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Windows;
using Whip.Common;
using static Whip.Common.Resources;
using System.Threading.Tasks;

namespace Whip.ViewModels.Tests.EditTrack
{
    [TestClass]
    public class EditTrackViewModelTests
    {
        private const string ArtistName = "Artist Name";
        private const string TrackTitle = "Track Title";
        private const string OriginalLyrics = "These are the original lyrics";
        private const string FoundLyrics = "These are the new lyrics";

        private Mock<Library> _mockLibrary;
        private Mock<IMessenger> _mockMessenger;
        private Mock<IAlbumInfoService> _mockAlbumInfoService;
        private Mock<ITrackUpdateService> _mockTrackUpdateService;
        private Mock<IImageProcessingService> _mockImageProcessingService;
        private Mock<IWebBrowserService> _mockWebBrowserService;
        private Mock<IFileDialogService> _mockFileDialogService;
        private Mock<ILyricsService> _mockLyricsService;

        private EditTrackViewModel GetSubjectUnderTest()
        {
            _mockLibrary = new Mock<Library>();
            _mockMessenger = new Mock<IMessenger>();
            _mockAlbumInfoService = new Mock<IAlbumInfoService>();
            _mockTrackUpdateService = new Mock<ITrackUpdateService>();
            _mockImageProcessingService = new Mock<IImageProcessingService>();
            _mockWebBrowserService = new Mock<IWebBrowserService>();
            _mockFileDialogService = new Mock<IFileDialogService>();
            _mockLyricsService = new Mock<ILyricsService>();

            return new EditTrackViewModel(_mockLibrary.Object, _mockMessenger.Object, _mockAlbumInfoService.Object,
                _mockTrackUpdateService.Object, _mockImageProcessingService.Object, _mockWebBrowserService.Object,
                _mockFileDialogService.Object, _mockLyricsService.Object);
        }

        [TestMethod]
        public void GetLyricsCommand_GivenLyricsFound_PopulatesLyrics()
        {
            // Arrange
            var track = GetTrack();
            var sut = GetSubjectUnderTest();
            _mockLyricsService.Setup(s => s.GetLyrics(ArtistName, TrackTitle)).ReturnsAsync(FoundLyrics);

            // Act
            sut.Edit(track);
            sut.GetLyricsCommand.Execute(null);

            // Assert
            Assert.AreEqual(FoundLyrics, sut.Track.Lyrics);
        }

        [TestMethod]
        public void GetLyricsCommand_GivenLyricsNotFound_DisplaysInformationMessage()
        {
            // Arrange
            var track = GetTrack();
            var sut = GetSubjectUnderTest();
            _mockLyricsService.Setup(s => s.GetLyrics(ArtistName, TrackTitle)).ReturnsAsync((string)null);

            // Act
            sut.Edit(track);
            sut.GetLyricsCommand.Execute(null);

            // Assert
            Assert.AreEqual(OriginalLyrics, sut.Track.Lyrics);
            _mockMessenger.Verify(m => m.Send(
                It.Is<ShowDialogMessage>(msg => IsLyricsNotFoundMessage(msg.ViewModel))));
        }

        [TestMethod]
        public void SaveCommand_GivenLyricsUpdated_SavesChanges()
        {
            // Arrange
            var track = GetTrack();
            var sut = GetSubjectUnderTest();

            // Act
            sut.Edit(track);
            sut.Track.Lyrics = FoundLyrics;
            sut.SaveCommand.Execute(null);

            // Assert
            _mockTrackUpdateService.Verify(s => s.SaveTrackChanges(
                It.Is<Track>(t => t.Lyrics == FoundLyrics),
                It.IsAny<Artist>(),
                It.IsAny<Disc>(),
                true,
                false,
                false,
                false
                ), Times.Once);
        }

        private bool IsLyricsNotFoundMessage(DialogViewModel model)
        {
            var message = model as MessageViewModel;

            return message != null
                && message.Icon == IconType.InfoCircle.ToString()
                && message.Title == EditTrackLyricsNotFoundTitle
                && message.Text == EditTrackLyricsNotFoundText;
        }

        private Track GetTrack()
        {
            var artist = new Artist
            {
                Name = ArtistName,
                Genre = "genre",
                Grouping = "grouping"
            };

            var album = new Album
            {
                Title = "Test Album",
                Year = "2000",
                DiscCount = 1
            };

            var disc = new Disc
            {
                DiscNo = 1,
                TrackCount = 10
            };

            var track = new Track
            {
                Title = TrackTitle,
                TrackNo = 1,
                Lyrics = OriginalLyrics,
                Year = "2000"
            };

            track.Artist = artist;
            artist.Tracks = new List<Track> { track };

            track.Disc = disc;
            disc.Tracks = new List<Track> { track };

            disc.Album = album;
            album.Discs = new List<Disc> { disc };

            album.Artist = artist;
            artist.Albums = new List<Album> { album };

            return track;
        }
    }
}
