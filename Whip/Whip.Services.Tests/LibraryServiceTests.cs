using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        [TestMethod]
        public void GetLibrary_ShouldReturnData()
        {
            // Arrange
            var _date1 = new DateTime(2000, 1, 1, 20, 13, 2);
            var _date2 = new DateTime(2005, 11, 16, 12, 7, 53);
            var _date3 = new DateTime(2009, 5, 27, 7, 52, 15);
            var testDirectoryPath = "test directory path";
            var testExtensionsList = new[] { ".docx", ".xlsx" };
            var files = new List<File>()
            {
                new File("testPath1", _date1, _date1),
                new File("testPath2", _date2, _date3),
                new File("testPath3", _date1, _date2),
            };

            var mockFileService = new Mock<IFileService>();
            var mockOrganiserService = new Mock<ILibraryDataOrganiserService>();
            var mockDataPersistenceService = new Mock<IDataPersistenceService>();

            mockFileService.Setup(fs => fs.GetFiles(It.IsAny<string>(), It.IsAny<string[]>())).Returns(files);

            var sut = new LibraryService(mockFileService.Object, mockOrganiserService.Object, mockDataPersistenceService.Object);

            // Act
            var result = sut.GetLibrary(testDirectoryPath, testExtensionsList);

            // Assert
            mockFileService.Verify(fs => fs.GetFiles(testDirectoryPath, testExtensionsList), Times.Once);
            mockOrganiserService.Verify(os => os.AddTrack(It.IsAny<string>(), It.IsAny<File>(), It.IsAny<ICollection<Artist>>()), Times.Exactly(files.Count));
        }
    }
}
