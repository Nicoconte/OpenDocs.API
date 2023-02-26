using Microsoft.EntityFrameworkCore;
using OpenDocs.API.Data;
using OpenDocs.API.Exceptions;
using OpenDocs.API.Models;

namespace OpenDocs.API.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        
        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateApplication(Applications app)
        {
            _context.Applications.Add(app);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> DeleteApplication(string appname)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(s => s.Name == appname);

            if (app is null) throw new ApplicationNotFoundException(appname);

            _context.Applications.Remove(app);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<Applications> GetApplicationByName(string appname)
        {
            return await _context.Applications.FirstOrDefaultAsync(c => c.Name == appname);
        }

        public async Task<List<Applications>> ListApplications(string groupId)
        {
            var query = _context.Applications.AsQueryable();

            if (!string.IsNullOrWhiteSpace(groupId))
                query = _context.Applications.Where(s => s.GroupId == groupId);

            var apps = await query.ToListAsync();

            return apps;
        }

        public async Task<bool> UpdateGroupId(string appname, string groupId)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(s => s.Name == appname);

            if (app is null) throw new ApplicationNotFoundException(appname);
            
            app.GroupId = groupId;

            _context.Applications.Update(app);

            var rows = await _context.SaveChangesAsync();


            return rows > 0;
        }
    }
}
