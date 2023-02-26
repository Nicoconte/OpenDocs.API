namespace OpenDocs.API.Services
{
    public interface IStorageService
    {
        public void CreateFile(string path, string content);
        public void CreateFile(string path, Stream file);
        public void DeleteFile(string path);
        public List<string> GetFilesFromFolder(string path);
        public void CreateFolder(string path, string foldername);
    }
}
