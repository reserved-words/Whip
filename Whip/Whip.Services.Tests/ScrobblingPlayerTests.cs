using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class ScrobblingPlayerTests
    {
        private const int MinimumUpdateNowPlayingDuration = 30;

        private readonly DateTime _testTime = new DateTime(2017,1,2,15,31,05);

        private Mock<IPlayer> _mockPlayer;
        private Mock<IScrobblingRules> _mockScrobblingRules;
        private Mock<IScrobbler> _mockScrobbler;
        private Mock<ICurrentDateTime> _mockCurrentDateTime;
        private Mock<IPlayProgressTracker> _mockPlayTimer;
        
        private ScrobblingPlayer GetSubjectUnderTest()
        {
            _mockPlayer = new Mock<IPlayer>();
            _mockScrobblingRules = new Mock<IScrobblingRules>();
            _mockScrobbler = new Mock<IScrobbler>();
            _mockCurrentDateTime = new Mock<ICurrentDateTime>();
            _mockPlayTimer = new Mock<IPlayProgressTracker>();

            _mockCurrentDateTime.Setup(c => c.Get()).Returns(_testTime);
            _mockScrobblingRules.Setup(r => r.MinimumUpdateNowPlayingDuration).Returns(MinimumUpdateNowPlayingDuration);
            _mockScrobblingRules.Setup(r => r.CanScrobble(It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            return new ScrobblingPlayer(_mockPlayer.Object, _mockScrobblingRules.Object, _mockScrobbler.Object, _mockCurrentDateTime.Object, _mockPlayTimer.Object);
        }

        [TestMethod]
        public void Pause_UpdatesScrobblingAndCallsBasePlayerMethod()
        {
            // Arrange
            var testTrack = new Track();
            var sut = GetSubjectUnderTest();

            // Act
            sut.Play(testTrack);
            sut.Pause();

            // Assert
            _mockPlayer.Verify(p => p.Pause(), Times.Once);
            _mockPlayTimer.Verify(t => t.Stop(), Times.Once);
            _mockScrobbler.Verify(s => s.UpdateNowPlayingAsync(testTrack, MinimumUpdateNowPlayingDuration));
        }

        [TestMethod]
        public void Play_GivenNullCurrentTrack_UpdatesScrobblingAndCallsBasePlayerMethod()
        {
            // Arrange
            var trackDuration = new TimeSpan(0, 2, 30);
            var trackDurationInSeconds = (int)trackDuration.TotalSeconds;
            var newTrack = new Track { Title = "Track 2", Duration = trackDuration };
            var sut = GetSubjectUnderTest();
            _mockPlayTimer.Setup(t => t.RemainingSeconds).Returns(trackDurationInSeconds);

            // Act
            sut.Play(null);
            sut.Play(newTrack);

            // Assert
            _mockPlayer.Verify(p => p.Play(newTrack), Times.Once);
            _mockPlayTimer.Verify(t => t.Stop(), Times.Never);
            _mockPlayTimer.Verify(t => t.StartNewTrack(trackDurationInSeconds), Times.Once);
            _mockScrobbler.Verify(s => s.UpdateNowPlayingAsync(newTrack, trackDurationInSeconds), Times.Once);
        }

        [TestMethod]
        public void Play_GivenNotNullCurrentTrack_UpdatesScrobblingAndCallsBasePlayerMethod()
        {
            // Arrange
            var trackDuration = new TimeSpan(0, 2, 30);
            var trackDurationInSeconds = (int)trackDuration.TotalSeconds;
            var originalTrack = new Track { Title = "Track 1" };
            var newTrack = new Track { Title = "Track 2", Duration = trackDuration };
            var sut = GetSubjectUnderTest();
            _mockPlayTimer.Setup(t => t.RemainingSeconds).Returns(trackDurationInSeconds);

            // Act
            sut.Play(originalTrack);
            sut.Play(newTrack);

            // Assert
            _mockPlayer.Verify(p => p.Play(newTrack), Times.Once);
            _mockPlayTimer.Verify(t => t.Stop(), Times.Once);
            _mockScrobbler.Verify(s => s.ScrobbleAsync(originalTrack, _testTime));
            _mockPlayTimer.Verify(t => t.StartNewTrack(trackDurationInSeconds), Times.Once);
            _mockScrobbler.Verify(s => s.UpdateNowPlayingAsync(newTrack, trackDurationInSeconds), Times.Once);
        }

        [TestMethod]
        public void Play_GivenNullNewTrack_UpdatesScrobblingAndCallsBasePlayerMethod()
        {
            // Arrange
            var originalTrack = new Track { Title = "Track 1" };
            var sut = GetSubjectUnderTest();
            
            // Act
            sut.Play(originalTrack);
            sut.Play(null);

            // Assert
            _mockPlayer.Verify(p => p.Play(null), Times.Once);
            _mockPlayTimer.Verify(t => t.Stop(), Times.Once);
            _mockScrobbler.Verify(s => s.ScrobbleAsync(originalTrack, _testTime));
            _mockScrobbler.Verify(s => s.UpdateNowPlayingAsync(originalTrack, MinimumUpdateNowPlayingDuration), Times.Once);
        }

        [TestMethod]
        public void Resume_UpdatesScrobblingAndCallsBasePlayerMethod()
        {
            // Arrange
            var testTrack = new Track();
            var testRemainingSeconds = 35;
            var sut = GetSubjectUnderTest();
            _mockPlayTimer.Setup(t => t.RemainingSeconds).Returns(testRemainingSeconds);

            // Act
            sut.Play(testTrack);
            sut.Resume();

            // Assert
            _mockPlayer.Verify(p => p.Resume(), Times.Once);
            _mockPlayTimer.Verify(t => t.Resume(), Times.Once);
            _mockScrobbler.Verify(s => s.UpdateNowPlayingAsync(testTrack, testRemainingSeconds));
        }

        [TestMethod]
        public void SkipToPercentage_UpdatesScrobblingAndCallsBasePlayerMethod()
        {
            // Arrange
            var testTrack = new Track();
            var testPercentage = 67;
            var testRemainingSeconds = 5;
            var sut = GetSubjectUnderTest();
            _mockPlayTimer.Setup(t => t.RemainingSeconds).Returns(testRemainingSeconds);

            // Act
            sut.Play(testTrack);
            sut.SkipToPercentage(testPercentage);

            // Assert
            _mockPlayer.Verify(p => p.SkipToPercentage(testPercentage), Times.Once);
            _mockPlayTimer.Verify(t => t.SkipToPercentage(testPercentage), Times.Once);
            _mockScrobbler.Verify(s => s.UpdateNowPlayingAsync(testTrack, testRemainingSeconds));
        }
    }
}
