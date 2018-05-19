using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whip.Common.Model;

namespace Whip.Services.Tests
{
    [TestClass]
    public class DirectoryNamerTests
    {
        [TestMethod]
        public void GetArtistDirectoryName_GivenArtistNameWithNoSpecialCharacters()
        {
            // Arrange
            var sut = new DirectoryNamer();
            var artistName = "ArtistName";
            var track = CreateTrack(artistName);

            // Act
            var result = sut.GetArtistDirectoryName(track);

            // Assert
            Assert.AreEqual("ArtistName", result);
        }

        [TestMethod]
        public void GetArtistDirectoryName_GivenArtistNameWithColon()
        {
            // Arrange
            var sut = new DirectoryNamer();
            var artistName = "Artist: Name";
            var track = CreateTrack(artistName);

            // Act
            var result = sut.GetArtistDirectoryName(track);

            // Assert
            Assert.AreEqual("Artist_ Name", result);
        }

        [TestMethod]
        public void GetAlbumDirectoryName_GivenAlbumTitleWithNoSpecialCharacters()
        {
            // Arrange
            var sut = new DirectoryNamer();
            var albumTitle = "AlbumTitle";
            var track = CreateTrack(albumTitle: albumTitle);

            // Act
            var result = sut.GetAlbumDirectoryName(track);

            // Assert
            Assert.AreEqual("AlbumTitle", result);
        }

        [TestMethod]
        public void GetAlbumDirectoryName_GivenAlbumTitleWithColon()
        {
            // Arrange
            var sut = new DirectoryNamer();
            var albumTitle = "Album:Title";
            var track = CreateTrack(albumTitle: albumTitle);

            // Act
            var result = sut.GetAlbumDirectoryName(track);

            // Assert
            Assert.AreEqual("Album_Title", result);
        }

        private static Track CreateTrack(string artistName = null, string albumTitle = null)
        {
            return new Track
            {
                Disc = new Disc
                {
                    Album = new Album
                    {
                        Title = albumTitle ?? "AlbumTitle",
                        Artist = new Artist
                        {
                            Name = artistName ?? "ArtistName"
                        }
                    }
                }
            };
        }
    }
}
