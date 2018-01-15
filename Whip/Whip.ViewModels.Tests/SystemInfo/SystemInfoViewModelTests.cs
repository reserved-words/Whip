using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Interfaces.Utilities;
using Whip.ViewModels.TabViewModels;
using static Whip.Common.Resources;

namespace Whip.ViewModels.Tests
{
    [TestClass]
    public class SystemInfoViewModelTests
    {
        private const string TestLastFmErrorMessage = "There is a problem!";
        private const string TestVersionNumber = "1.2";

        private readonly DateTime _testCurrentDate = new DateTime(2017,1,1);
        private readonly DateTime _testPublishDate = new DateTime(2016,1,9);
        private readonly DateTime _testLogsDate = new DateTime(2015,1,15);

        private SystemInfoViewModel GetSubjectUnderTest(bool offline = false, bool lastFmStatus = true)
        {
            var mockWebServicesStatus = new Mock<IWebServicesStatus>();
            mockWebServicesStatus.Setup(u => u.InternetStatus).Returns(!offline);
            mockWebServicesStatus.Setup(u => u.LastFmStatus).Returns(lastFmStatus);
            mockWebServicesStatus.Setup(u => u.LastFmErrorMessage).Returns(TestLastFmErrorMessage);

            var mockApplicationInfoService = new Mock<IApplicationInfoService>();
            mockApplicationInfoService.Setup(s => s.Version).Returns(TestVersionNumber);
            mockApplicationInfoService.Setup(s => s.PublishDate).Returns(_testPublishDate);

            var mockCurrentDateTime = new Mock<ICurrentDateTime>();
            mockCurrentDateTime.Setup(c => c.Get()).Returns(_testCurrentDate);

            var mockLogRepository = new Mock<ILogRepository>();
            mockLogRepository.Setup(r => r.GetLogs(_testLogsDate)).Returns(GetTestLogs());

            return new SystemInfoViewModel(mockWebServicesStatus.Object, mockApplicationInfoService.Object,
                mockCurrentDateTime.Object, mockLogRepository.Object);
        }

        [TestMethod]
        public void ApplicationInfo_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Assert
            Assert.AreEqual(TestVersionNumber, sut.ApplicationVersion);
            Assert.AreEqual(_testPublishDate.ToString(ApplicationPublishedFormat), sut.ApplicationPublishDate);
        }

        [TestMethod]
        public void Logs_GivenLogsDateChanged_AreUpdated()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.LogsDate = _testLogsDate;

            // Assert
            sut.Logs.ShouldBeEquivalentTo(GetTestLogs());
            sut.Logs.Should().BeInDescendingOrder(l => l.Id);
        }

        private IEnumerable<Log> GetTestLogs()
        {
            return new List<Log>
            {
                new Log { LoggedAt = _testLogsDate.AddMinutes(36), Message = "Test message 1", Id = 2 },
                new Log { LoggedAt = _testLogsDate.AddMinutes(2056), Message = "Test message 2", Id = 1 }
            };
        }

    }
}
