using OpenDocs.API.Models;

namespace OpenDocs.API.Services
{
    public interface IApplicationService
    {
        public Task<bool> CreateApplication(Applications app);
        public Task<bool> DeleteApplication(string appname);
        public Task<bool> UpdateGroupId(string appname, string groupId);
        public Task<List<Applications>> ListApplications(string groupId);
        public Task<Applications> GetApplicationByName(string appname);
    }
}
