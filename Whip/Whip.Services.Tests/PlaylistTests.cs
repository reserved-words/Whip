using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class PlaylistTests
    {
        private List<Track> _testTracks;
        private List<Track> _testTracksInOrder;
        private List<Track> _testTracksShuffled;

        private Mock<ITrackQueue> _mockTrackQueue;
        private Mock<IRandomTrackSorter> _mockRandomTrackSorter;
        private Mock<IDefaultTrackSorter> _mockDefaultTrackSorter;

        [TestInitialize]
        public void Initialise()
        {
            _testTracks = new List<Track>
            {
                new Track { Title = "Track 1" },
                new Track { Title = "Track 2" },
                new Track { Title = "Track 3" }
            };

            _testTracksInOrder = _testTracks.OrderBy(t => t.Title).ToList();
            _testTracksShuffled = _testTracks.OrderByDescending(t => t.Title).ToList();
        }

        private Playlist GetSubjectUnderTest()
        {
            _mockTrackQueue = new Mock<ITrackQueue>();
            _mockRandomTrackSorter = new Mock<IRandomTrackSorter>();
            _mockDefaultTrackSorter = new Mock<IDefaultTrackSorter>();

            _mockDefaultTrackSorter.Setup(s => s.Sort(It.IsAny<IEnumerable<Track>>())).Returns(_testTracksInOrder);
            _mockRandomTrackSorter.Setup(s => s.Sort(It.IsAny<IEnumerable<Track>>())).Returns(_testTracksShuffled);

            return new Playlist(_mockTrackQueue.Object, _mockDefaultTrackSorter.Object, _mockRandomTrackSorter.Object);
        }

        [TestMethod]
        public void MoveNext_UpdatesCurrentTrack()
        {
            // Arrange
            var currentTrack = new Track {Title = "Track 1"};
            var newTrack = new Track { Title = "Track 2" };
 
            var subjectUnderTest = GetSubjectUnderTest();
            _mockTrackQueue.Setup(q => q.CurrentTrack).Returns(newTrack);
            subjectUnderTest.CurrentTrackChanged += t => { currentTrack = t; };

            // Act
            subjectUnderTest.MoveNext();

            // Assert
            _mockTrackQueue.Verify(q => q.MoveNext(), Times.Once);
            currentTrack.Should().Be(newTrack);
        }

        [TestMethod]
        public void MovePrevious_UpdatesCurrentTrack()
        {
            // Arrange
            var currentTrack = new Track { Title = "Track 1" };
            var newTrack = new Track { Title = "Track 2" };

            var subjectUnderTest = GetSubjectUnderTest();
            _mockTrackQueue.Setup(q => q.CurrentTrack).Returns(newTrack);
            subjectUnderTest.CurrentTrackChanged += t => { currentTrack = t; };

            // Act
            subjectUnderTest.MovePrevious();

            // Assert
            _mockTrackQueue.Verify(q => q.MovePrevious(), Times.Once);
            currentTrack.Should().Be(newTrack);
        }

        [TestMethod]
        public void Set_GivenShuffle_UpdatesQueue()
        {
            // Arrange
            const string testPlaylistName = "Playlist 123";
            const int startIndex = 1;

            var listUpdated = false;
            var subjectUnderTest = GetSubjectUnderTest();
            subjectUnderTest.ListUpdated += () => { listUpdated = true; };

            // Act
            subjectUnderTest.Set(testPlaylistName, _testTracks, _testTracks[startIndex], true);

            // Assert
            subjectUnderTest.PlaylistName.Should().Be(testPlaylistName);
            subjectUnderTest.Tracks.Should().BeEquivalentTo(_testTracks);
            _mockTrackQueue.Verify(q => q.Set(It.Is<List<Track>>(t => t.SequenceEqual(_testTracksShuffled)), startIndex, true), Times.Once);
            listUpdated.Should().BeTrue();
        }

        [TestMethod]
        public void Set_GivenNoShuffle_UpdatesQueue()
        {
            // Arrange
            const string testPlaylistName = "Playlist 123";
            const int startIndex = 1;

            var listUpdated = false;
            var subjectUnderTest = GetSubjectUnderTest();
            subjectUnderTest.ListUpdated += () => { listUpdated = true; };

            // Act
            subjectUnderTest.Set(testPlaylistName, _testTracks, _testTracks[startIndex], false);

            // Assert
            subjectUnderTest.PlaylistName.Should().Be(testPlaylistName);
            subjectUnderTest.Tracks.Should().BeEquivalentTo(_testTracks);
            _mockTrackQueue.Verify(q => q.Set(It.Is<List<Track>>(t => t.SequenceEqual(_testTracksInOrder)), startIndex, true), Times.Once);
            listUpdated.Should().BeTrue();
        }

        [TestMethod]
        public void Set_GivenCurrentTrackIsNotNull_DoesNotMoveToNextTrack()
        {
            // Arrange
            var currentTrack = new Track { Title = "Track 1" };
            
            var currentTrackChanged = false;
            var subjectUnderTest = GetSubjectUnderTest();
            subjectUnderTest.CurrentTrackChanged += t => { currentTrackChanged = true; };

            _mockTrackQueue.Setup(q => q.CurrentTrack).Returns(currentTrack);

            // Act
            subjectUnderTest.Set("", _testTracks, null, false);

            // Assert
            _mockTrackQueue.Verify(q => q.MoveNext(), Times.Never);
            currentTrackChanged.Should().BeFalse();
        }

        [TestMethod]
        public void Set_GivenCurrentTrackIsNull_MovesToNextTrack()
        {
            // Arrange
            var currentTrackChanged = false;
            var subjectUnderTest = GetSubjectUnderTest();
            subjectUnderTest.CurrentTrackChanged += t => { currentTrackChanged = true; };

            _mockTrackQueue.Setup(q => q.CurrentTrack).Returns((Track)null);

            // Act
            subjectUnderTest.Set("", _testTracks, null, false);

            // Assert
            _mockTrackQueue.Verify(q => q.MoveNext(), Times.Once);
            currentTrackChanged.Should().BeTrue();
        }
    }
}
