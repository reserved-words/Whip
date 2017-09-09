using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Singletons;
using static Whip.Common.Resources;

namespace Whip.Services.Tests
{
    [TestClass]
    public class NewFilePlayerTests
    {
        private const string TestFilePath = "testOriginalFilePath";
        private const string TestCurrentlyPlayingFilePath = "testNewFilePath";

        private Mock<IPlayer> _mockBasePlayer;
        private Mock<IFileService> _mockFileService;
        
        private NewFilePlayer GetSubjectUnderTest()
        {
            _mockBasePlayer = new Mock<IPlayer>();
            _mockFileService = new Mock<IFileService>();

            _mockFileService.Setup(f => f.CopyFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(TestCurrentlyPlayingFilePath);

            return new NewFilePlayer(_mockBasePlayer.Object, _mockFileService.Object);
        }

        [TestMethod]
        public void Pause_CallsBasePlayerMethod()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Pause();

            // Assert
            _mockBasePlayer.Verify(p => p.Pause(), Times.Once);
        }

        [TestMethod]
        public void Play_GivenNullTrack_CallsBasePlayerMethodAndDeletesFiles()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Play(null);

            // Assert
            _mockBasePlayer.Verify(p => p.Play(null), Times.Once);
            _mockFileService.Verify(s => s.DeleteFiles(CurrentPlayingDirectoryName, ""), Times.Once);
        }

        [TestMethod]
        public void Play_GivenNotNullTrack_CallsBasePlayerMethodAndDeletesFiles()
        {
            // Arrange
            var trackToPlay = new Track
            {
                File = new File(TestFilePath, "", DateTime.MinValue, DateTime.MinValue)
            };
            var sut = GetSubjectUnderTest();

            // Act
            sut.Play(trackToPlay);

            // Assert
            _mockFileService.Verify(s => s.CopyFile(TestFilePath, CurrentPlayingDirectoryName));
            _mockBasePlayer.Verify(p => p.Play(It.Is<Track>(t => t.File.FullPath == TestCurrentlyPlayingFilePath)), Times.Once);
            _mockFileService.Verify(s => s.DeleteFiles(CurrentPlayingDirectoryName, TestCurrentlyPlayingFilePath), Times.Once);
        }

        [TestMethod]
        public void Resume_CallsBasePlayerMethod()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Resume();

            // Assert
            _mockBasePlayer.Verify(p => p.Resume(), Times.Once);
        }


        [TestMethod]
        public void SkipToPercentage_CallsBasePlayerMethod()
        {
            // Arrange
            var testPercentage = 30;
            var sut = GetSubjectUnderTest();

            // Act
            sut.SkipToPercentage(testPercentage);

            // Assert
            _mockBasePlayer.Verify(p => p.SkipToPercentage(testPercentage), Times.Once);
        }
    }
}
