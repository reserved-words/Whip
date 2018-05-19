using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;

namespace Whip.Services.Tests
{
    [TestClass]
    public class TrackFilterServiceTests
    {
        private Mock<Library> _mockLibrary;
        private Mock<IDefaultTrackSorter> _mockTrackSorter;

        private readonly Func<IEnumerable<Track>, IEnumerable<Track>> _defaultSort = ts => ts.OrderBy(t => t.Title);

        private TrackFilterService GetSubjectUnderTest()
        {
            _mockLibrary = new Mock<Library>();
            _mockTrackSorter = new Mock<IDefaultTrackSorter>();

            _mockTrackSorter.Setup(s => s.Sort(It.IsAny<IEnumerable<Track>>())).Returns(_defaultSort);

            return new TrackFilterService(_mockLibrary.Object, _mockTrackSorter.Object);
        }

        [TestMethod]
        public void GetAlbumTracksByArtist_ReturnsAlbumTracksByArtistInDefaultOrder()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var artist = GetMockArtist();
            var tracks = artist.Albums
                .SelectMany(a => a.Discs)
                .SelectMany(d => d.Tracks);
            var expectedResult = _defaultSort(tracks).ToList();

            // Act
            var result = sut.GetAlbumTracksByArtist(artist);

            // Assert
            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }

        private Artist GetMockArtist()
        {
            return new Artist
            {
                Albums = new List<Album>
                {
                    GetAlbum("A1", "A2", "A3"),
                    GetAlbum("C1", "C2", "C3"),
                    GetAlbum("B1", "B2", "B3")
                }
            };
        }

        private Album GetAlbum(params string[] trackTitles)
        {
            var album = new Album { Title = trackTitles[0] };
            var disc = new Disc();
            album.Discs.Add(disc);
            foreach (var title in trackTitles)
            {
                disc.Tracks.Add(new Track { Title = title });
            }
            return album;
        }
    }
}
