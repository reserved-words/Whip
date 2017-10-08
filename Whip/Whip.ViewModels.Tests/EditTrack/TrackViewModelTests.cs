using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels.EditTrack;

namespace Whip.ViewModels.Tests.EditTrack
{
    [TestClass]
    public class TrackViewModelTests
    {
        private const string TagToRemove = "any tag";

        [TestMethod]
        public void RemoveTagCommand_OnExecute_RemovesTagAndSetsTrackModified()
        {
            // Arrange
            var mockMessenger = new Mock<IMessenger>();
            var mockAlbumInfoService = new Mock<IAlbumInfoService>();
            var mockImageProcessingService = new Mock<IImageProcessingService>();
            var mockWebBrowserService = new Mock<IWebBrowserService>();
            var mockFileDialogService = new Mock<IFileDialogService>();
            var artists = new List<Artist>();
            var tags = new List<string> { TagToRemove };
            var track = new Track
            {
                Disc = new Disc
                {
                    Album = new Album
                    {
                        Artist = new Artist()
                    }
                },
                Artist = new Artist(),
                Tags = new List<string> { "some tag", TagToRemove, "some other tag" }
            };

            var sut = new TrackViewModel(mockMessenger.Object, mockAlbumInfoService.Object, mockImageProcessingService.Object,
                    mockWebBrowserService.Object, mockFileDialogService.Object, artists, tags, track);

            // Act
            sut.RemoveTagCommand.Execute(TagToRemove);

            // Assert
            Assert.AreEqual(2, sut.Tags.Count);
            Assert.IsFalse(sut.Tags.Contains(TagToRemove));
            Assert.IsTrue(sut.Modified);
        }
    }
}
