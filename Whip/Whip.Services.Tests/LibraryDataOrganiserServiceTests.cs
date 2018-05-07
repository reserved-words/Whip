using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class LibraryDataOrganiserServiceTests
    {
        private const string AlbumArtistName = "AA";
        private const string TrackArtistName = "TA";
        private const string AlbumTitle = "AT";
        private const int DiscNo = 2;

        private Library _library;
        private Mock<ITaggingService> _taggingService;
        private Mock<ICommentProcessingService> _commentProcessingService;
        private Mock<ILibrarySortingService> _sortingService;

        private LibraryDataOrganiserService GetSubjectUnderTest()
        {
            _library = new Library();
            _sortingService = new Mock<ILibrarySortingService>();
            _taggingService = new Mock<ITaggingService>();
            _commentProcessingService = new Mock<ICommentProcessingService>();

            return new LibraryDataOrganiserService(_taggingService.Object, _commentProcessingService.Object, _library, _sortingService.Object);
        }

        [TestMethod]
        public void RemoveTrack_GivenTracksRemainOnDisc()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(5, 0, 0, 200, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsTrue(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsTrue(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenNoTracksRemainOnDisc()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(0, 1, 0, 200, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsFalse(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsTrue(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenNoDiscsRemainOnAlbum()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(0, 0, 3, 200, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsFalse(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsFalse(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenAlbumArtistHasNoAlbumsButHasTracksRemaining()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(0, 0, 0, 200, 0, 6);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsFalse(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsFalse(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenAlbumArtistHasNoTracksButHasAlbumsRemaining()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(0, 0, 1, 200, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsFalse(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsFalse(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenAlbumArtistHasNoTracksOrAlbumsRemaining()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(0, 0, 0, 200, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsFalse(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsFalse(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsFalse(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenTrackArtistHasNoAlbumsButHasTracksRemaining()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(1, 0, 0, 10, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsTrue(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsTrue(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenTrackArtistHasNoTracksButHasAlbumsRemaining()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(1, 0, 0, 0, 2, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsTrue(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsTrue(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsTrue(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        [TestMethod]
        public void RemoveTrack_GivenTrackArtistHasNoTracksOrAlbumsRemaining()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var track = GetTrack(1, 0, 0, 0, 0, 0);

            // Act
            sut.RemoveTrack(track);

            // Assert
            Assert.IsFalse(track.Disc.Tracks.Contains(track));
            Assert.IsTrue(track.Disc.Album.Discs.Contains(track.Disc));
            Assert.IsTrue(track.Disc.Album.Artist.Albums.Contains(track.Disc.Album));
            Assert.IsFalse(track.Artist.Tracks.Contains(track));
            Assert.IsFalse(_library.Artists.Contains(track.Artist));
            Assert.IsTrue(_library.Artists.Contains(track.Disc.Album.Artist));
        }

        private Track GetTrack(int otherTracksOnDisc, int otherDiscsOnAlbum, int otherAlbumsByAlbumArtist, int otherTracksByTrackArtist,
            int albumsByTrackArtist, int tracksByAlbumArtist)
        {
            var trackArtist = new Artist
            {
                Name = TrackArtistName
            };

            var albumArtist = new Artist
            {
                Name = AlbumArtistName
            };

            var album = new Album
            {
                Artist = albumArtist,
                Title = AlbumTitle
            };

            var disc = new Disc
            {
                Album = album,
                DiscNo = DiscNo
            };

            var track = new Track
            {
                Artist = trackArtist,
                Disc = disc
            };

            disc.Tracks.Add(track);
            album.Discs.Add(disc);
            albumArtist.Albums.Add(album);
            trackArtist.Tracks.Add(track);

            for (var i = 0; i < otherTracksOnDisc; i++)
            {
                disc.Tracks.Add(new Track { Disc = disc });
            }

            for (var i = 0; i < otherDiscsOnAlbum; i++)
            {
                album.Discs.Add(new Disc { Album = album });
            }

            for (var i = 0; i < otherAlbumsByAlbumArtist; i++)
            {
                albumArtist.Albums.Add(new Album { Artist = albumArtist });
            }

            for (var i = 0; i < otherTracksByTrackArtist; i++)
            {
                trackArtist.Tracks.Add(new Track { Artist = trackArtist });
            }

            for (var i = 0; i < albumsByTrackArtist; i++)
            {
                trackArtist.Albums.Add(new Album { Artist = trackArtist });
            }

            for (var i = 0; i < tracksByAlbumArtist; i++)
            {
                albumArtist.Tracks.Add(new Track { Artist = albumArtist });
            }

            _library.Artists.Add(trackArtist);
            _library.Artists.Add(albumArtist);

            return track;
        }
    }
}
