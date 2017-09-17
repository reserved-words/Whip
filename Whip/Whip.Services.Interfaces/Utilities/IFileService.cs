using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IFileService
    {
        FilesWithStatus GetFiles(string directory, List<string> extensions, DateTime lastUpdated);
        List<File> GetAllFiles(string directory, List<string> extensions);
        string CopyFile(string copyFromPath, string copyToDirectoryName);
        void DeleteFiles(string directoryName, params string[] excludeFiles);
        string[] GetFileContent(string directoryName, string filename);
    }
}
