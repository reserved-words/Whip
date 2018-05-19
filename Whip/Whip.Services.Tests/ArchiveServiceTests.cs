using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Singletons;
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
        private const string FullPath2 = "asfasasfa2";
        private const string RelativePath1 = "asfasaadadsdssfa";
        private const string RelativePath2 = "asfasaadadsdssfa2";

        private Mock<IFileService> _fileService;
        private Mock<ITaggingService> _taggingService;
        private Mock<IConfigSettings> _configSettings;
        private Mock<IUserSettings> _userSettings;
        private Mock<ILibraryService> _libraryService;
        private Mock<IPlaylistsService> _playlistsService;
        private Library _library;
        private Mock<IPlaylist> _playlist;
        private Mock<IDirectoryNamer> _directoryNamer;

        private ArchiveService GetSubjectUnderTest()
        {
            _fileService = new Mock<IFileService>();
            _taggingService = new Mock<ITaggingService>();
            _configSettings = new Mock<IConfigSettings>();
            _userSettings = new Mock<IUserSettings>();
            _libraryService = new Mock<ILibraryService>();
            _library = new Library();
            _playlistsService = new Mock<IPlaylistsService>();
            _playlist = new Mock<IPlaylist>();
            _directoryNamer = new Mock<IDirectoryNamer>();

            return new ArchiveService(
                _userSettings.Object, 
                _fileService.Object, 
                _taggingService.Object,
                _configSettings.Object,
                _libraryService.Object,
                _library,
                _playlistsService.Object,
                _playlist.Object,
                _directoryNamer.Object);
        }
        
        [TestMethod]
        public void ArchiveTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var tracks = GetTracks();
            var dir1 = "asf";
            _directoryNamer
                .Setup(d => d.GetArtistDirectoryName(It.IsAny<Track>())).Returns<Track>(t => t.Disc.Album.Artist.Name);
            _directoryNamer
                .Setup(d => d.GetAlbumDirectoryName(It.IsAny<Track>())).Returns<Track>(t => t.Disc.Album.Title);
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
            _fileService.Verify(s => s.CreateDirectory(ArchiveDirectory, ArtistName1, AlbumTitle1), Times.Exactly(tracks.Count));
            foreach (var track in tracks)
            {
                _fileService.Verify(s => s.CopyFile(track.File.FullPath, dir1, null), Times.Once);
            }
            _libraryService.Verify(s => s.RemoveTracks(_library, tracks), Times.Once);
            _playlistsService.Verify(s => s.RemoveTracks(tracks), Times.Once);
            _playlist.Verify(p => p.RemoveTracks(tracks), Times.Once);
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
            _fileService.Verify(s => s.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _libraryService.Verify(s => s.RemoveTracks(It.IsAny<Library>(), It.IsAny<List<Track>>()), Times.Never);
            _playlistsService.Verify(s => s.RemoveTracks(It.IsAny<List<Track>>()), Times.Never);
        }

        private static List<Track> GetTracks()
        {
            return new List<Track>
            {
                new Track
                {
                    Title = "Track 1",
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
                },
                new Track
                {
                    Title = "Track 2",
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
                    File = new File(FullPath2, RelativePath2, DateTime.MinValue, DateTime.MinValue)
                }
            };
        }
    }
}
