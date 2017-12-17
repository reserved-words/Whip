using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.TabViewModels;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class CurrentPlaylistViewModelTests
    {
        private Mock<IPlaylist> _mockPlaylist;
        private Mock<IPlayRequestHandler> _mockPlayRequestHandler;
        private Mock<Library> _mockLibrary;
        private Mock<TrackContextMenuViewModel> _mockContextMenuViewModel;

        private CurrentPlaylistViewModel GetSubjectUnderTest()
        {
            _mockPlaylist = new Mock<IPlaylist>();
            _mockPlayRequestHandler = new Mock<IPlayRequestHandler>();
            _mockLibrary = new Mock<Library>();
            _mockContextMenuViewModel = new Mock<TrackContextMenuViewModel>();

            return new CurrentPlaylistViewModel(_mockPlaylist.Object, _mockLibrary.Object, _mockContextMenuViewModel.Object, _mockPlayRequestHandler.Object);
        }

        [TestMethod]
        public void PlayCommand_ShouldRequestPlayFromSelectedTrack()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var tracks = new List<Track>
            {
                new Track { Title = "Track 1" },
                new Track { Title = "Track 2" },
                new Track { Title = "Track 3" }
            };
            _mockPlaylist.Setup(p => p.Tracks).Returns(tracks);
            sut.SelectedTrack = tracks[1];

            // Act
            sut.PlayCommand.Execute(null);

            // Assert
            _mockPlayRequestHandler.Verify(h => h.MoveToTrack(tracks[1]));
        }
    }
}
