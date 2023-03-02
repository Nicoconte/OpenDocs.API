using OpenDocs.API.Models;
using Environments = OpenDocs.API.Models.Environments;

namespace OpenDocs.API.Services
{
    public interface ISettingService
    {
        public Task<bool> InitSettings();
        public Task<List<Environments>> GetEnvironments();
        public Task<Settings> GetSettings();
        public Task<bool> UpdateRetentionDays(int days);
        public Task<bool> UpdateStorageBasePath(string path);
    }
}
