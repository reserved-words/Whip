using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Singletons;
using static Whip.Common.Resources;
using System.IO;

namespace Whip.Services.Tests
{
    [TestClass]
    public class NewFilePlayerTests
    {
        private const string TestFilePath = @"C:\Users\Username\DirectoryName\testOriginalFilePath";
        private const string TestCurrentlyPlayingFilePath = @"C:\Users\Username\DirectoryName\testNewFilePath";

        private readonly DateTime _currentTime = new DateTime(2017,1,2,15,30,16);

        private Mock<ICurrentDateTime> _mockCurrentDateTime;
        private Mock<IPlayer> _mockBasePlayer;
        private Mock<IFileService> _mockFileService;
        
        private NewFilePlayer GetSubjectUnderTest()
        {
            _mockCurrentDateTime = new Mock<ICurrentDateTime>();
            _mockBasePlayer = new Mock<IPlayer>();
            _mockFileService = new Mock<IFileService>();

            _mockFileService.Setup(f => f.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(TestCurrentlyPlayingFilePath);

            _mockCurrentDateTime.Setup(c => c.Get()).Returns(_currentTime);

            return new NewFilePlayer(_mockCurrentDateTime.Object, _mockBasePlayer.Object, _mockFileService.Object);
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
                File = new Common.Model.File(TestFilePath, "", DateTime.MinValue, DateTime.MinValue)
            };
            var sut = GetSubjectUnderTest();

            // Act
            sut.Play(trackToPlay);

            // Assert
            _mockFileService.Verify(s => s.CopyFile(TestFilePath, CurrentPlayingDirectoryName, $"copy_{_currentTime.Ticks}"));
            _mockBasePlayer.Verify(p => p.Play(It.Is<Track>(t => t.File.FullPath == TestCurrentlyPlayingFilePath)), Times.Once);
            _mockFileService.Verify(s => s.DeleteFiles(CurrentPlayingDirectoryName, Path.GetFileName(TestCurrentlyPlayingFilePath)), Times.Once);
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
