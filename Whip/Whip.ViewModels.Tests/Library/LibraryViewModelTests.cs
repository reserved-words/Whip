using Moq;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels;
using Whip.ViewModels.TabViewModels.Library;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.Tests
{
    public class LibraryViewModelTests
    {
        private Mock<Library> _mockLibrary;
        private Mock<ILibrarySortingService> _mockLibrarySortingService;
        private Mock<IPlayRequestHandler> _mockPlayRequestHandler;
        private Mock<LibraryArtistViewModel> _mockArtistViewModel;
        private Mock<LibraryTracksViewModel> _mockTracksViewModel;

        private LibraryViewModel GetSubjectUnderTest()
        {
            _mockLibrary = new Mock<Library>();
            _mockLibrarySortingService = new Mock<ILibrarySortingService>();
            _mockPlayRequestHandler = new Mock<IPlayRequestHandler>();
            _mockArtistViewModel = new Mock<LibraryArtistViewModel>();
            _mockTracksViewModel = new Mock<LibraryTracksViewModel>();

            return new LibraryViewModel(_mockLibrary.Object, _mockLibrarySortingService.Object, _mockPlayRequestHandler.Object,
                _mockArtistViewModel.Object, _mockTracksViewModel.Object);
        }


    }
}
