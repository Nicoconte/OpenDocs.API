using Microsoft.EntityFrameworkCore;
using OpenDocs.API.Data;
using OpenDocs.API.Exceptions;
using OpenDocs.API.Models;

namespace OpenDocs.API.Services
{
    public class SettingService : ISettingService
    {
        private readonly ApplicationDbContext _context;

        public SettingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> GetEnviroments()
        {
            return new List<string>() { "Development", "Testing", "Production" };
        }

        public async Task<Settings> GetSettings()
        {
            return (await _context.Settings.FirstOrDefaultAsync()) ?? throw new EntityNotFoundException(nameof(Settings));
        }

        public async Task<bool> InitSettings()
        {
            if ((await _context.Settings.ToListAsync()).Count == 0)
            {
                _context.Settings.Add(new Settings()
                {
                    StorageBasePath = "C:/GIT/storage/",
                    RetentionDays = 15
                });
            }

            var envs = await _context.Environments.ToListAsync();

            foreach(var defEnv in GetEnviroments())
            {
                if (!envs.Any(c => c.EnvironmentType == defEnv))
                {
                    _context.Environments.Add(new Models.Environments
                    {
                        EnvironmentType = defEnv,
                        IsActive = true,
                        AccessKey = Guid.NewGuid().ToString(),
                    });
                }
            }

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> UpdateRetentionDays(int days)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();

            if (setting is null) 
                throw new EntityNotFoundException(nameof(Settings));
            
            setting.RetentionDays = days;
            setting.UpdatedAt = DateTime.Now;

            _context.Settings.Update(setting);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> UpdateStorageBasePath(string path)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();

            if (setting is null) 
                throw new EntityNotFoundException(nameof(Settings));

            setting.StorageBasePath = path;
            setting.UpdatedAt = DateTime.Now;

            _context.Settings.Update(setting);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }
    }
}
