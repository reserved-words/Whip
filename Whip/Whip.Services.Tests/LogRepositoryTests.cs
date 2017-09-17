using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Utilities;
using static Whip.Common.Resources;

namespace Whip.Services.Tests
{
    [TestClass]
    public class LogRepositoryTests
    {
        private readonly string[] _testFileContent = new[] {"", "", ""};

        private readonly DateTime _testDate = new DateTime(2017,1,1);
        private readonly List<Log> _testLogs = new List<Log> 
        {
            new Log { LoggedAt = new DateTime(2017,1,1), Message = "test" }
        };

        private LogRepository GetSubjectUnderTest(bool fileExists)
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService
                .Setup(s => s.GetFileContent(LogFileDirectory, string.Format(LogFileNameFormat, _testDate)))
                .Returns(fileExists ? _testFileContent : null);
            var mockLogParser = new Mock<ILogParser>();
            mockLogParser.Setup(p => p.Parse(_testFileContent)).Returns(_testLogs);
            return new LogRepository(mockFileService.Object, mockLogParser.Object);
        }

        [TestMethod]
        public void GetLogs_GivenLogFileExists_ReturnsParsedLogs()
        {
            // Arrange
            var sut = GetSubjectUnderTest(true);

            // Act
            var result = sut.GetLogs(_testDate);

            // Assert
            result.ShouldBeEquivalentTo(_testLogs);
        }

        [TestMethod]
        public void GetLogs_GivenNoLogFileExists_ReturnsEmptyList()
        {
            // Arrange
            var sut = GetSubjectUnderTest(false);

            // Act
            var result = sut.GetLogs(_testDate);

            // Assert
            result.Should().HaveCount(0);
        }
    }
}
