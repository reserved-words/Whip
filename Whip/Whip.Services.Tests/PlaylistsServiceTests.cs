using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class PlaylistsServiceTests
    {
        private const string RemoveFilePath1 = "RemoveFilePath1";
        private const string RemoveFilePath2 = "RemoveFilePath2";
        private const string RemoveFilePath3 = "RemoveFilePath3";
        private const string RemoveFilePath4 = "RemoveFilePath4";
        private const string RemoveFilePath5 = "RemoveFilePath5";

        private const string KeepFilePath1 = "KeepFilePath1";
        private const string KeepFilePath2 = "KeepFilePath2";
        private const string KeepFilePath3 = "KeepFilePath3";
        
        private readonly DateTime _testDate = new DateTime(2000, 1, 1);

        private Mock<IPlaylistRepository> _repository;
        private readonly OrderedPlaylist _playlist1 = new OrderedPlaylist(1, "Playlist 1", true)
        {
            Tracks = new List<string> { KeepFilePath1, KeepFilePath2, KeepFilePath3 }
        };

        private readonly OrderedPlaylist _playlist2 = new OrderedPlaylist(2, "Playlist 2", true)
        {
            Tracks = new List<string> { KeepFilePath1, RemoveFilePath2, KeepFilePath2, RemoveFilePath4, KeepFilePath3 }
        };

        private PlaylistsService GetSubjectUnderTest()
        {
            _repository = new Mock<IPlaylistRepository>();
            _repository.Setup(r => r.GetOrderedPlaylists()).Returns(new List<OrderedPlaylist> { _playlist1, _playlist2 });
            return new PlaylistsService(_repository.Object);
        }

        [TestMethod]
        public void RemoveFromAllPlaylists()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var tracks = GetTracks();

            // Act
            sut.RemoveTracks(tracks);

            // Assert
            Assert.AreEqual(3, _playlist2.Tracks.Count);
            Assert.IsFalse(_playlist2.Tracks.Contains(RemoveFilePath2));
            Assert.IsFalse(_playlist2.Tracks.Contains(RemoveFilePath4));
            Assert.IsTrue(_playlist2.Tracks.Contains(KeepFilePath1));
            Assert.IsTrue(_playlist2.Tracks.Contains(KeepFilePath2));
            Assert.IsTrue(_playlist2.Tracks.Contains(KeepFilePath3));
            _repository.Verify(r => r.Save(_playlist2), Times.Once);
            _repository.Verify(r => r.Save(_playlist1), Times.Never);
        }

        private List<Track> GetTracks()
        {
            return new List<Track>
            {
                new Track { File = new File(RemoveFilePath1, "", _testDate, _testDate) },
                new Track { File = new File(RemoveFilePath2, "", _testDate, _testDate) },
                new Track { File = new File(RemoveFilePath3, "", _testDate, _testDate) },
                new Track { File = new File(RemoveFilePath4, "", _testDate, _testDate) },
                new Track { File = new File(RemoveFilePath5, "", _testDate, _testDate) }
            };
        }
    }
}
