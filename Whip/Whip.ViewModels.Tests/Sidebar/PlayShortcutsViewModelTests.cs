using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.ViewModels.Sidebar.Tests
{
    [TestClass]
    public class PlayShortcutsViewModelTests
    {
        private Mock<IPlayRequestHandler> _mockPlayRequestHandler;
        private Mock<IPlaylistRepository> _mockRepository;
        private Mock<ITrackSearchService> _mockTrackSearchService;

        private PlayShortcutsViewModel GetSubjectUnderTest()
        {
            _mockPlayRequestHandler = new Mock<IPlayRequestHandler>();
            _mockRepository = new Mock<IPlaylistRepository>();
            _mockTrackSearchService = new Mock<ITrackSearchService>();

            return new PlayShortcutsViewModel(_mockPlayRequestHandler.Object, _mockRepository.Object, _mockTrackSearchService.Object);
        }
        
        [TestMethod]
        public void Groupings_AreUpdatedOnLibraryUpdate()
        {
            //// Arrange
            //var sut = GetSubjectUnderTest();
            //sut.Groupings = new List<string>();

            //// Act
            //_mockLibrary.Raise(l => l.Updated += null, It.IsAny<Track>());

            //// Assert
            //sut.Groupings.Should().BeEquivalentTo("Grouping 1", "Grouping 2", "Grouping 3");
        }

        [TestMethod]
        public void PlayGroupingCommand_SendsPlayRequest()
        {
            //// Arrange
            //var testGrouping = "some grouping";
            //var sut = GetSubjectUnderTest();

            //// Act
            //sut.PlayGroupingCommand.Execute(testGrouping);

            //// Assert
            //_mockPlayRequestHandler.Verify(p => p.PlayGrouping(testGrouping, SortType.Random, null), Times.Once);
        }

        [TestMethod]
        public void ShuffleLibraryCommand_SendsPlayRequest()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.ShuffleLibraryCommand.Execute(0);

            // Assert
            _mockPlayRequestHandler.Verify(p => p.PlayAll(SortType.Random, null), Times.Once);
        }
    }
}
