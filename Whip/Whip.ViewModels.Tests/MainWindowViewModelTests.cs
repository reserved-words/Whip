using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Interfaces;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void OnExit_StopsPlayerAndSavesLibrary()
        {
            // Arrange
            var mockLibrary = new Mock<Library>();
            var mockLibraryService = new Mock<ILibraryService>();
            var mockPlayer = new Mock<IPlayer>();
            var mockPlaylist = new Mock<IPlaylist>();
            var sut = new MainWindowViewModel(mockLibraryService.Object, null, mockLibrary.Object, null, 
                mockPlaylist.Object, null, null, mockPlayer.Object);

            // Act
            sut.OnExit();

            // Assert
            mockLibraryService.Verify(s => s.SaveLibrary(mockLibrary.Object), Times.Once);
            mockPlayer.Verify(p => p.Stop(), Times.Once);
        }
    }
}
