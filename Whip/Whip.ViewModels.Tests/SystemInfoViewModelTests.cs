using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Services.Interfaces;
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

        private readonly DateTime _testPublishDate = new DateTime(2016,1,9);

        private SystemInfoViewModel GetSubjectUnderTest(bool offline = false, bool lastFmStatus = true)
        {
            var mockUserSettings = new Mock<IUserSettings>();
            var mockApplicationInfoService = new Mock<IApplicationInfoService>();
            mockUserSettings.Setup(u => u.Offline).Returns(offline);
            mockUserSettings.Setup(u => u.LastFmStatus).Returns(lastFmStatus);
            mockUserSettings.Setup(u => u.LastFmErrorMessage).Returns(TestLastFmErrorMessage);
            mockApplicationInfoService.Setup(s => s.Version).Returns(TestVersionNumber);
            mockApplicationInfoService.Setup(s => s.PublishDate).Returns(_testPublishDate);
            return new SystemInfoViewModel(mockUserSettings.Object, mockApplicationInfoService.Object);
        }

        [TestMethod]
        public void InternetStatus_GivenOffline_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest(offline: true);

            // Assert
            Assert.AreEqual(Offline, sut.InternetStatus);
            Assert.AreEqual(OfflineErrorMessageDetails, sut.InternetStatusDetails);
        }

        [TestMethod]
        public void InternetStatus_GivenNotOffline_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Assert
            Assert.AreEqual(Online, sut.InternetStatus);
            Assert.AreEqual(string.Empty, sut.InternetStatusDetails);
        }

        [TestMethod]
        public void LastFmStatus_GivenLastFmOn_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Assert
            Assert.AreEqual(true, sut.LastFmOn);
            Assert.AreEqual(On, sut.LastFmStatus);
            Assert.AreEqual(string.Empty, sut.LastFmStatusDetails);
        }

        [TestMethod]
        public void LastFmStatus_GivenLastFmOff_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest(lastFmStatus: false);

            // Assert
            Assert.AreEqual(false, sut.LastFmOn);
            Assert.AreEqual(Off, sut.LastFmStatus);
            Assert.AreEqual(LastFmOffErrorMessageDetails, sut.LastFmStatusDetails);
        }

        [TestMethod]
        public void LastFmErrorMessage_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Assert
            Assert.AreEqual(TestLastFmErrorMessage, sut.LastFmErrorMessage);
        }

        [TestMethod]
        public void ApplicationInfo_IsCorrect()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Assert
            Assert.AreEqual(string.Format(ApplicationVersionFormat, TestVersionNumber), sut.ApplicationVersion);
            Assert.AreEqual(string.Format(ApplicationPublishedFormat,_testPublishDate), sut.ApplicationPublishDate);
        }
    }
}
