using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Singletons;
using Whip.ViewModels.Singletons.Interfaces;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void PopulateLibrary_ShouldCallServices()
        {
            // Arrange
            var mockLibraryService = new Mock<ILibraryService>();
            var mockUserSettingsService = new Mock<IUserSettingsService>();
            var mockLibrary = new Mock<ILibrary>();
            var testMusicDirectoryPath = "test music directory path";
            var testArtists = new List<Artist> { new Artist() };
            
            mockUserSettingsService.Setup(uss => uss.MusicDirectory).Returns(testMusicDirectoryPath);
            mockLibraryService.Setup(ls => ls.GetLibrary(It.IsAny<string>(), It.IsAny<string[]>())).Returns(testArtists);
            
            var sut = new MainWindowViewModel(mockLibraryService.Object, mockUserSettingsService.Object, mockLibrary.Object);

            // Act
            sut.PopulateLibraryCommand.Execute(null);

            // Assert
            mockLibraryService.Verify(ls => ls.GetLibrary(testMusicDirectoryPath, ApplicationSettings.FileExtensions), Times.Once);
            mockLibrary.VerifySet(l => l.Artists = testArtists, Times.Once);
        }
    }
}
