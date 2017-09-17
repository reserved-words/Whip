using System;
using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using static Whip.Common.Resources;

namespace Whip.Services.Utilities
{
    public class LogRepository : ILogRepository
    {
        private readonly IFileService _fileService;
        private readonly ILogParser _logParser;

        public LogRepository(IFileService fileService, ILogParser logParser)
        {
            _fileService = fileService;
            _logParser = logParser;
        }

        public IEnumerable<Log> GetLogs(DateTime date)
        {
            var logFileContent = _fileService.GetFileContent(LogFileDirectory, string.Format(LogFileNameFormat, date));
            return logFileContent == null
                ? new List<Log>() 
                : _logParser.Parse(logFileContent);
        }
    }
}
