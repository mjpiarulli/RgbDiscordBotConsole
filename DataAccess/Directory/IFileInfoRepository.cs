using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess.Directory
{
    public interface IFileInfoRepository
    {
        IEnumerable<FileInfo> SearchThroughDirectoriesForFiles(string path, Func<FileInfo, bool> query);
        int GetFileCount(string path, string searchPattern);
        byte[] SafelyReadFile(string path);
        bool WriteFile(string path, byte[] fileBytes);
    }
}
