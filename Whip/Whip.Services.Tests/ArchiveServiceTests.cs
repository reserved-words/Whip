using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using static Whip.Common.Resources;

namespace Whip.Services.Tests
{
    [TestClass]
    public class ArchiveServiceTests
    {
        private const string ArchiveDirectory = "sagasgasg";
        private const string AlbumTitle1 = "asgsag";
        private const string ArtistName1 = "weywye";
        private const string FullPath1 = "asfasasfa";
        private const string RelativePath1 = "asfasaadadsdssfa";

        private Mock<IFileService> _fileService;
        private Mock<ITaggingService> _taggingService;
        private Mock<IConfigSettings> _configSettings;
        private Mock<IUserSettings> _userSettings;

        private ArchiveService GetSubjectUnderTest()
        {
            _fileService = new Mock<IFileService>();
            _taggingService = new Mock<ITaggingService>();
            _configSettings = new Mock<IConfigSettings>();
            _userSettings = new Mock<IUserSettings>();

            return new ArchiveService(
                _userSettings.Object, 
                _fileService.Object, 
                _taggingService.Object,
                _configSettings.Object);
        }
        
        [TestMethod]
        public void ArchiveTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var tracks = GetTracks();
            var dir1 = "asf";
            _fileService
                .Setup(s => s.CreateDirectory(ArchiveDirectory, ArtistName1, AlbumTitle1))
                .Returns(dir1);
            _userSettings
                .Setup(u => u.ArchiveDirectory).Returns(ArchiveDirectory);
            string errorMessage;

            // Act
            var result = sut.ArchiveTracks(tracks, out errorMessage);

            // Assert
            Assert.AreEqual(null, errorMessage);
            Assert.IsTrue(result);
            foreach (var track in tracks)
            {
                _fileService.Verify(s => s.CreateDirectory(ArchiveDirectory, ArtistName1, AlbumTitle1), Times.Once);
                _fileService.Verify(s => s.CopyFile(track.File.FullPath, dir1), Times.Once);
                _fileService.Verify(s => s.DeleteFile(track.File.FullPath, true, true), Times.Once);
            }
        }

        [TestMethod]
        public void ArchiveTracks_GivenNoArchiveDirectorySaved()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var tracks = GetTracks();
            string errorMessage;
            _userSettings.Setup(u => u.ArchiveDirectory).Returns("");

            // Act
            var result = sut.ArchiveTracks(tracks, out errorMessage);

            // Assert
            Assert.AreEqual(ErrorNoArchiveDirectorySet, errorMessage);
            Assert.IsFalse(result);
            _fileService.Verify(s => s.CreateDirectory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileService.Verify(s => s.CopyFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileService.Verify(s => s.DeleteFile(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        private List<Track> GetTracks()
        {
            return new List<Track>
            {
                new Track
                {
                    Disc = new Disc
                    {
                        DiscNo = 1,
                        Album = new Album
                        {
                            Title = AlbumTitle1,
                            Artist = new Artist
                            {
                                Name = ArtistName1
                            }
                        }
                    },
                    File = new File(FullPath1, RelativePath1, DateTime.MinValue, DateTime.MinValue)
                }
            };
        }
    }
}
