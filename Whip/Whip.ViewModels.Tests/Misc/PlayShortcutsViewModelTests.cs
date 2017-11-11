using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class PlayShortcutsViewModelTests
    {
        private Mock<Library> _mockLibrary;
        private Mock<IPlayRequestHandler> _mockPlayRequestHandler;

        private PlayShortcutsViewModel GetSubjectUnderTest()
        {
            _mockLibrary = new Mock<Library>();
            _mockPlayRequestHandler = new Mock<IPlayRequestHandler>();

            _mockLibrary.Object.Artists = new List<Artist>
            {
                new Artist {Name = "Artist1", Grouping = "Grouping 1"},
                new Artist {Name = "Artist2", Grouping = null},
                new Artist {Name = "Artist3", Grouping = "Grouping 2"},
                new Artist {Name = "Artist4", Grouping = "Grouping 1"},
                new Artist {Name = "Artist5", Grouping = ""},
                new Artist {Name = "Artist6", Grouping = "Grouping 3"}
            };

            return new PlayShortcutsViewModel(_mockLibrary.Object, _mockPlayRequestHandler.Object);
        }
        
        [TestMethod]
        public void Groupings_AreUpdatedOnLibraryUpdate()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.Groupings = new List<string>();

            // Act
            _mockLibrary.Raise(l => l.Updated += null, It.IsAny<Track>());

            // Assert
            sut.Groupings.Should().BeEquivalentTo("Grouping 1", "Grouping 2", "Grouping 3");
        }

        [TestMethod]
        public void PlayGroupingCommand_SendsPlayRequest()
        {
            // Arrange
            var testGrouping = "some grouping";
            var sut = GetSubjectUnderTest();

            // Act
            sut.PlayGroupingCommand.Execute(testGrouping);

            // Assert
            _mockPlayRequestHandler.Verify(p => p.PlayGrouping(testGrouping, SortType.Random, null), Times.Once);
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
