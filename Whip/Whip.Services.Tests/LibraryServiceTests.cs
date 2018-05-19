using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private const string AlbumTitle1 = "asgsag";
        private const string ArtistName1 = "weywye";
        private const string FullPath1 = "asfasasfa";
        private const string FullPath2 = "asfasasfa2";
        private const string RelativePath1 = "asfasaadadsdssfa";
        private const string RelativePath2 = "asfasaadadsdssfa2";

        private Mock<IFileService> _fileService;
        private Mock<ILibraryDataOrganiserService> _libraryDataOrganiserService;
        private Mock<ILibrarySortingService> _librarySortingService;
        private Mock<ITrackRepository> _dataPersistenceService;
        private Mock<ITaggingService> _taggingService;
        private Mock<IUserSettings> _userSettings;
        private Mock<IConfigSettings> _configSettings;

        private LibraryService GetSubjectUnderTest()
        {
            _fileService = new Mock<IFileService>();
            _libraryDataOrganiserService = new Mock<ILibraryDataOrganiserService>();
            _librarySortingService = new Mock<ILibrarySortingService>();
            _dataPersistenceService = new Mock<ITrackRepository>();
            _taggingService = new Mock<ITaggingService>();
            _userSettings = new Mock<IUserSettings>();
            _configSettings = new Mock<IConfigSettings>();

            return new LibraryService(_fileService.Object, _libraryDataOrganiserService.Object, _taggingService.Object, 
                _dataPersistenceService.Object, _userSettings.Object, _librarySortingService.Object, _configSettings.Object);
        }

        [TestMethod]
        public void RemoveTracks()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var tracks = GetTracks();
            var library = new Library();
            var libraryUpdateInvoked = false;
            library.Updated += track =>
            {
                libraryUpdateInvoked = track == null;
            };

            // Act
            sut.RemoveTracks(library, tracks);

            // Assert

            foreach (var track in tracks)
            {
                _fileService.Verify(s => s.DeleteFile(track.File.FullPath, true, true), Times.Once);
                _libraryDataOrganiserService.Verify(s => s.RemoveTrack(track), Times.Once);
            }

            _dataPersistenceService.Verify(d => d.Save(library), Times.Once);

            Assert.IsTrue(libraryUpdateInvoked);
        }

        private static List<Track> GetTracks()
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
                },
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
                    File = new File(FullPath2, RelativePath2, DateTime.MinValue, DateTime.MinValue)
                }
            };
        }
    }
}
