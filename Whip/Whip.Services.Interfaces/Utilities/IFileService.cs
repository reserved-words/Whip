using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IFileService
    {
        FilesWithStatus GetFiles(string directory, string[] extensions, DateTime lastUpdated);
        List<File> GetAllFiles(string directory, string[] extensions);
    }
}
