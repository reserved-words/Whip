using FluentAssertions;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Whip.Common;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.Sidebar.Tests
{
    [TestClass]
    public class SettingsIconsViewModelTests
    {
        private const int SoundIcon = 2;
        private const int VolumeDownIcon = 3;
        private const int VolumeUpIcon = 4;

        private Mock<IPlayerVolume> _mockPlayer;
        private Mock<IMessenger> _mockMessenger;
        private Mock<IUserSettings> _mockUserSettings;

        private SettingsIconsViewModel GetSubjectUnderTest()
        {
            _mockPlayer = new Mock<IPlayerVolume>();
            _mockMessenger = new Mock<IMessenger>();
            _mockUserSettings = new Mock<IUserSettings>();

            return new SettingsIconsViewModel(_mockMessenger.Object, _mockPlayer.Object, _mockUserSettings.Object);
        }

        [TestMethod]
        public void SoundOffCommand_MutesPlayerAndUpdatesIcon()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var soundCommandIcon = sut.IconCommands[SoundIcon];
            soundCommandIcon.On = true;

            // Act
            soundCommandIcon.Command.Execute(soundCommandIcon);

            // Assert
            _mockPlayer.Verify(p => p.Mute(), Times.Once);
            Assert.IsFalse(soundCommandIcon.On);
        }

        [TestMethod]
        public void SoundOnCommand_UnmutesPlayerAndUpdatesIcon()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var soundCommandIcon = sut.IconCommands[SoundIcon];
            soundCommandIcon.On = false;

            // Act
            soundCommandIcon.Command.Execute(soundCommandIcon);

            // Assert
            _mockPlayer.Verify(p => p.Unmute(), Times.Once);
            Assert.IsTrue(soundCommandIcon.On);
        }

        [TestMethod]
        public void VolumeDownCommand_DecreasesVolumeBy10()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.IconCommands[SoundIcon].On = false;
            var volumeDownCommandIcon = sut.IconCommands[VolumeDownIcon];
            _mockPlayer.Setup(p => p.GetVolumePercentage()).Returns(50);

            // Act
            volumeDownCommandIcon.Command.Execute(volumeDownCommandIcon);

            // Assert
            _mockPlayer.Verify(p => p.SetVolumePercentage(40), Times.Once);
            Assert.IsTrue(sut.IconCommands[SoundIcon].On);
        }

        [TestMethod]
        public void VolumeUpCommand_IncreasesVolumeBy10()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            sut.IconCommands[SoundIcon].On = false;
            var volumeUpCommandIcon = sut.IconCommands[VolumeUpIcon];
            _mockPlayer.Setup(p => p.GetVolumePercentage()).Returns(50);

            // Act
            volumeUpCommandIcon.Command.Execute(volumeUpCommandIcon);

            // Assert
            _mockPlayer.Verify(p => p.SetVolumePercentage(60), Times.Once);
            Assert.IsTrue(sut.IconCommands[SoundIcon].On);
        }
    }
}
