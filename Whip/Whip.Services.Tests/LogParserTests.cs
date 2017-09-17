using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whip.Common.Model;

namespace Whip.Services.Tests
{
    [TestClass]
    public class LogParserTests
    {
        private const string LogFormatSeparator = "|";

        [TestMethod]
        public void Parse_ReturnsParsedLogs()
        {
            // Arrange
            var sut = new LogParser();
            
            // Act
            var result = sut.Parse(GetTestLogsContent());

            // Assert
            result.ShouldBeEquivalentTo(GetLogs());
        }

        private string[] GetTestLogsContent()
        {
            return GetLogs()
                .Select(l => $"{l.LoggedAt:yyyy-MM-dd HH:mm:ss}{LogFormatSeparator}{l.Type}{LogFormatSeparator}{l.Message}")
                .ToArray();
        }

        private List<Log> GetLogs()
        {
            return new List<Log>
            {
                new Log { Id = 1, LoggedAt = new DateTime(2017,9,17,7,57,7), Type = "INFO", Message = "Message 1" },
                new Log { Id = 2, LoggedAt = new DateTime(2017,9,17,7,58,36), Type = "INFO", Message = "Message 2" },
                new Log { Id = 3, LoggedAt = new DateTime(2017,9,17,8,25,11), Type = "INFO", Message = "Message 3" }
            };
        }
    }
}
