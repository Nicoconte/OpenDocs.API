using OpenDocs.API.Data;
using OpenDocs.API.Exceptions;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;

namespace OpenDocs.API.Services
{
    public class StorageService : IStorageService
    {
        public void CreateFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public void DeleteFile(string path)
        {
            if (!File.Exists(path))
                throw new DocumentNotFoundException(path);

            File.Delete(path);
        }

        public List<string> GetFilesFromFolder(string path)
        {
            if (!Directory.Exists(path))
                throw new FolderNotFoundException(path);

            return Directory.GetFiles(path).Select(c => c.Replace(path, string.Empty)).ToList();
        }

        public void CreateFolder(string path, string foldername)
        {
            string fullpath = string.Concat(path.TrimEnd('/'), "/", foldername);

            if (Directory.Exists(fullpath)) return;

            Directory.CreateDirectory(fullpath);
        }

        public async void CreateFile(string path, Stream file)
        {
            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var filename = Path.GetFileName(path);

            var directoryFiles = Directory.GetFiles(path.Replace($"/{filename}", string.Empty));

            var lastFileAfterCopy = directoryFiles.Length >= 2 
                ? directoryFiles[directoryFiles.Length - 2] 
                : directoryFiles[directoryFiles.Length - 1];

            if (directoryFiles.Length >= 2 && File.ReadAllBytes(path).SequenceEqual(File.ReadAllBytes(lastFileAfterCopy)))
            {
                File.Delete(path);
            }
        }
    }
}
