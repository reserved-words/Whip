using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class PlayProgressTrackerTests
    {
        private const int TotalTrackDuration = 265;

        private readonly DateTime _startTime = new DateTime(2017, 1, 1, 15, 15, 28);

        [TestMethod]
        public void StartNewTrack_UpdatesAllValues()
        {
            // Arrange
            var mockCurrentDateTime = new Mock<ICurrentDateTime>();
            var sut = new PlayProgressTracker(mockCurrentDateTime.Object);

            // Act
            sut.StartNewTrack(TotalTrackDuration);

            // Assert
            sut.RemainingSeconds.Should().Be(TotalTrackDuration);
            sut.TotalTrackDurationInSeconds.Should().Be(TotalTrackDuration);
            sut.SecondsOfTrackPlayed.Should().Be(0);
        }

        [TestMethod]
        public void PauseAfterPlay_UpdatesAllValues()
        {
            // Arrange
            var pauseAt = _startTime.AddSeconds(TotalTrackDuration);
            var mockCurrentDateTime = new Mock<ICurrentDateTime>();
            mockCurrentDateTime.SetupSequence(c => c.Get())
                .Returns(_startTime)
                .Returns(pauseAt);

            var sut = new PlayProgressTracker(mockCurrentDateTime.Object);

            // Act
            sut.StartNewTrack(TotalTrackDuration);
            sut.Pause();

            // Assert
            sut.RemainingSeconds.Should().Be(TotalTrackDuration - TotalTrackDuration);
            sut.TotalTrackDurationInSeconds.Should().Be(TotalTrackDuration);
            sut.SecondsOfTrackPlayed.Should().Be(TotalTrackDuration);
        }

        [TestMethod]
        public void PauseAfterResume_UpdatesAllValues()
        {
            // Arrange
            var firstPlayDuration = 65;
            var pauseDuration = 20;
            var secondPlayDuration = 10;

            var firstPauseAt = _startTime.AddSeconds(firstPlayDuration);
            var resumeAt = firstPauseAt.AddSeconds(pauseDuration);
            var secondPauseAt = resumeAt.AddSeconds(secondPlayDuration);

            var mockCurrentDateTime = new Mock<ICurrentDateTime>();
            mockCurrentDateTime.SetupSequence(c => c.Get())
                .Returns(_startTime)
                .Returns(firstPauseAt)
                .Returns(resumeAt)
                .Returns(secondPauseAt);

            var sut = new PlayProgressTracker(mockCurrentDateTime.Object);

            // Act
            sut.StartNewTrack(TotalTrackDuration);
            sut.Pause();
            sut.Resume();
            sut.Pause();

            // Assert
            sut.RemainingSeconds.Should().Be(TotalTrackDuration - firstPlayDuration - secondPlayDuration);
            sut.TotalTrackDurationInSeconds.Should().Be(TotalTrackDuration);
            sut.SecondsOfTrackPlayed.Should().Be(firstPlayDuration + secondPlayDuration);
        }

        [TestMethod]
        public void SkipToPercentage_UpdatesAllValues()
        {
            // Arrange
            var percentageSkippedTo = 34;
            var secondsSkippedTo = TotalTrackDuration * percentageSkippedTo / 100;
            
            var mockCurrentDateTime = new Mock<ICurrentDateTime>();
            var sut = new PlayProgressTracker(mockCurrentDateTime.Object);

            // Act
            sut.StartNewTrack(TotalTrackDuration);
            sut.SkipToPercentage(percentageSkippedTo);

            // Assert
            sut.RemainingSeconds.Should().Be(TotalTrackDuration - secondsSkippedTo);
            sut.TotalTrackDurationInSeconds.Should().Be(TotalTrackDuration);
            sut.SecondsOfTrackPlayed.Should().Be(secondsSkippedTo);
        }

        [TestMethod]
        public void PauseAfterSkipToPercentage_UpdatesAllValues()
        {
            // Arrange
            var firstPlayDuration = 65;
            var secondPlayDuration = 10;
            var percentageSkippedTo = 85;
            var secondsSkippedTo = TotalTrackDuration * percentageSkippedTo / 100;

            var skipAt = _startTime.AddSeconds(firstPlayDuration);
            var pauseAt = skipAt.AddSeconds(secondPlayDuration);

            var mockCurrentDateTime = new Mock<ICurrentDateTime>();
            mockCurrentDateTime.SetupSequence(c => c.Get())
                .Returns(_startTime)
                .Returns(skipAt)
                .Returns(pauseAt);

            var sut = new PlayProgressTracker(mockCurrentDateTime.Object);

            // Act
            sut.StartNewTrack(TotalTrackDuration);
            sut.SkipToPercentage(percentageSkippedTo);
            sut.Pause();

            // Assert
            sut.RemainingSeconds.Should().Be(TotalTrackDuration - secondsSkippedTo - secondPlayDuration);
            sut.TotalTrackDurationInSeconds.Should().Be(TotalTrackDuration);
            sut.SecondsOfTrackPlayed.Should().Be(secondsSkippedTo + secondPlayDuration);
        }
    }
}
