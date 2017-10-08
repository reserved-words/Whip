using System;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whip.Common.Model;
using Whip.XmlDataAccess;
using static Whip.Common.Resources;

namespace Whip.ExternalLibraries.Tests
{
    [TestClass]
    public class TrackXmlParserTests
    {
        private const string TrackRelativeFilePath = @"Stony Sleep\A Slack Romance\01 Midmay.mp3";
        private const string TrackFullFilePath = @"C:\Users\rhian\Google Drive\Music\Stony Sleep\A Slack Romance\01 Midmay.mp3";
        private const string TrackTitle = "Midmay";
        private const string TrackYear = "1999";
        private const string TrackTags = "";
        private const string TrackLyrics = "Some lyrics here";
        private const int TrackNo = 1;

        private readonly DateTime _trackDateModified = new DateTime(2016,8,14,9,8,59);
        private readonly TimeSpan _trackDuration = new TimeSpan(0, 3, 25);

        [TestMethod]
        public void GetTrack_ReturnsCorrectXmlElement()
        {
            // Arrange
            var sut = new TrackXmlParser();
            var xml = GetTrackElement();
            var disc = new Disc();
            var artist = new Artist();

            // Act
            var result = sut.GetTrack(xml, disc, artist);

            // Assert
            Assert.AreEqual(TrackRelativeFilePath, result.File.RelativePath);
            Assert.AreEqual(TrackFullFilePath, result.File.FullPath);
            Assert.AreEqual(_trackDateModified, result.File.DateModified);
            Assert.AreEqual(TrackTitle, result.Title);
            Assert.AreEqual(TrackNo, result.TrackNo);
            Assert.AreEqual(_trackDuration, result.Duration);
            Assert.AreEqual(TrackYear, result.Year);
            Assert.AreEqual(0, result.Tags.Count);
            Assert.AreEqual(TrackLyrics, result.Lyrics);
        }

        private XElement GetTrackElement()
        {
            var xml = new XElement(PropertyNames.Track);
            xml.Add(new XAttribute(PropertyNames.RelativeFilepath, TrackRelativeFilePath));
            xml.Add(new XAttribute(PropertyNames.FullFilepath, TrackFullFilePath));
            xml.Add(new XAttribute(PropertyNames.DateModified, _trackDateModified.ToString(StandardDateFormat, CultureInfo.InvariantCulture)));
            xml.Add(new XAttribute(PropertyNames.Title, TrackTitle));
            xml.Add(new XAttribute(PropertyNames.TrackNo, TrackNo));
            xml.Add(new XAttribute(PropertyNames.Duration, _trackDuration.ToString(StandardTimeSpanFormat)));
            xml.Add(new XAttribute(PropertyNames.TrackYear, TrackYear));
            xml.Add(new XAttribute(PropertyNames.Tags, TrackTags));
            xml.Add(new XAttribute(PropertyNames.Lyrics, TrackLyrics));
            return xml;
        }
    }
}
