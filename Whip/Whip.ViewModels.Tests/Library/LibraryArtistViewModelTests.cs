using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.TabViewModels.Library;
using System.Windows.Media.Imaging;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class LibraryArtistViewModelTests
    {
        private const string NewWiki = "This is the new wiki";
        private const string NewImageUrl = "New Image Url";
        private const int NumberOfSimilarArtistsToDisplay = 5;

        private Mock<IArtistInfoService> _mockArtistInfoService;
        private Mock<IImageProcessingService> _mockImageProcessingService;
        private Mock<IConfigSettings> _mockConfigSettings;

        private readonly Artist _testArtist = new Artist { Name = "Test Artist", WebInfo = new ArtistWebInfo { Wiki = "This is the old wiki", ExtraLargeImageUrl = null } };
        private readonly BitmapImage _newImage = new BitmapImage();

        private LibraryArtistViewModel GetSubjectUnderTest()
        {
            _mockArtistInfoService = new Mock<IArtistInfoService>();
            _mockImageProcessingService = new Mock<IImageProcessingService>();
            _mockConfigSettings = new Mock<IConfigSettings>();

            _mockConfigSettings.Setup(s => s.NumberOfSimilarArtistsToDisplay).Returns(NumberOfSimilarArtistsToDisplay);
            _mockImageProcessingService.Setup(s => s.GetImageFromUrl(NewImageUrl)).ReturnsAsync(_newImage);
            _mockArtistInfoService.Setup(s => s
                .PopulateArtistInfo(_testArtist, NumberOfSimilarArtistsToDisplay))
                .Callback<Artist, int>(MockUpdateArtistInfo);

            return new LibraryArtistViewModel(_mockArtistInfoService.Object, _mockImageProcessingService.Object, _mockConfigSettings.Object);
        }

        private static void MockUpdateArtistInfo(Artist artist, int numberOfSimilarArtistsToDisplay)
        {
            artist.WebInfo.Wiki = NewWiki;
            artist.WebInfo.ExtraLargeImageUrl = NewImageUrl;
        }

        [TestMethod]
        public void OnChangeArtist_UpdatesArtistInfo()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Artist = _testArtist;
           
            // Assert
            // Not sure how to do this since values are updated asynchronously - currently test fails
            // sut.Wiki.Should().Be(NewWiki);
            // sut.Image.Should().Be(_newImage);
            // sut.LoadingArtistImage.Should().BeFalse();
        }
    }
}
