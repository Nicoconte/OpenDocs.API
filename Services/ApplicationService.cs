using Microsoft.EntityFrameworkCore;
using OpenDocs.API.Data;
using OpenDocs.API.Exceptions;
using OpenDocs.API.Models;
using System.Text.RegularExpressions;

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

        public async Task SetCurrentModificationDate(string appname)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(s => s.Name == appname);

            if (app is null) throw new ApplicationNotFoundException(appname);

            app.UpdatedAt = DateTime.Now;

            _context.Applications.Update(app);

            await _context.SaveChangesAsync();
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

        public async Task<List<Applications>> GetAllApplications(int? startIndex = 0, int? quantity = 10, string? name = "")
        {
            var query = _context.Applications.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.ToLower().Contains(name.ToLower()));
            }

            if (startIndex != null && quantity != null)
            {
                query = query.Skip(startIndex.Value).Take(quantity.Value);
            }


            var apps = await query.ToListAsync();

            return apps;
        }

        public async Task<List<string>> GetAllGroups(int? startIndex = 0, int? quantity = 10, string? name = "")
        {
            var query = _context.Applications.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.GroupId.ToLower().Contains(name.ToLower()));
            }

            if (startIndex != null && quantity != null)
            {
                query = query.Skip(startIndex.Value).Take(quantity.Value);
            }


            var groups = (await query.ToListAsync()).Select(s => s.GroupId).Distinct().ToList();

            return groups;
        }

        public async Task<Applications> GetApplicationByName(string appname)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Name == appname);
            return app;
        }

        public async Task<List<Applications>> GetApplicationsByGroup(string groupId)
        {
            return await _context.Applications.Where(s => s.GroupId == groupId).ToListAsync();
        }
    }
}
