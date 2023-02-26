using OpenDocs.API.Models;

namespace OpenDocs.API.Services
{
    public interface ISettingService
    {
        public Task<bool> InitSettings();
        public List<string> GetEnviroments();
        public Task<Settings> GetSettings();
        public Task<bool> UpdateRetentionDays(int days);
        public Task<bool> UpdateStorageBasePath(string path);
    }
}
