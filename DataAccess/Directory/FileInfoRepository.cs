using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess.Directory
{
    public class FileInfoRepository : IFileInfoRepository
    {
        public IEnumerable<FileInfo> SearchThroughDirectoriesForFiles(string path, Func<FileInfo, bool> query)
        {
            var files = System.IO.Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                .Select(f => new FileInfo(f))
                .Where(query);
            foreach (var file in files)
            {
                yield return file;
            }

            foreach (var folder in System.IO.Directory.GetDirectories(path))
            {
                foreach (var file in SearchThroughDirectoriesForFiles(folder, query))
                    yield return file;
            }
        }

        public int GetFileCount(string path, string searchPattern)
        {
            var fileCount = System.IO.Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly)
                .Select(f => new FileInfo(f))
                .Count();

            return fileCount;
        }

        public byte[] SafelyReadFile(string path)
        {
            byte[] fileBytes;
            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[2048];
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    fileBytes = ms.ToArray();
                }
            }

            return fileBytes;
        }

        public bool WriteFile(string path, byte[] fileBytes)
        {
            var basePath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(basePath) && !System.IO.Directory.Exists(basePath))
                System.IO.Directory.CreateDirectory(basePath);

            File.WriteAllBytes(path, fileBytes);

            return true;
        }
    }
}