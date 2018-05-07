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
        string CopyFile(string copyFromPath, string copyToDirectoryName, string copyFileName = null);
        void DeleteFiles(string directoryName, params string[] excludeFiles);
        string[] GetFileContent(string directoryName, string filename);
        string CreateDirectory(string directory, params string[] subdirectories);
        void DeleteFile(string filePath, bool deleteParentDirIfEmpty = false, bool deleteGranparentDirIfEmpty = false);
    }
}
