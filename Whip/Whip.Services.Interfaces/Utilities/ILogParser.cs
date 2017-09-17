using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ILogParser
    {
        IEnumerable<Log> Parse(string[] logFileContent);
    }
}
