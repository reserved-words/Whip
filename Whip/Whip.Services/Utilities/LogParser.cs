using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LogParser : ILogParser
    {
        private const char LogFormatSeparator = '|';
        
        public IEnumerable<Log> Parse(string[] logFileContent)
        {
            var count = 0;

            foreach (var line in logFileContent)
            {
                var split = line.Split(LogFormatSeparator);
                yield return new Log
                {
                    Id = ++count,
                    LoggedAt = DateTime.Parse(split[0]),
                    Type = split[1],
                    Message = split[2]
                };
            }
        }
    }
}
