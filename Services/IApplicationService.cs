using OpenDocs.API.Models;

namespace OpenDocs.API.Services
{
    public interface IApplicationService
    {
        public Task<List<Applications>> GetAllApplications(int? startIndex=0, int? quantity=10, string? name = "");
        public Task<Applications> GetApplicationByName(string appname);
        public Task<bool> CreateApplication(Applications app);
        public Task<bool> DeleteApplication(string appname);
        public Task SetCurrentModificationDate(string appname); 

        public Task<List<string>> GetAllGroups(int? startIndex=0, int? quantity=10, string? name="");
        public Task<List<Applications>> GetApplicationsByGroup(string groupId);
        public Task<bool> UpdateGroupId(string appname, string groupId);
    }
}
