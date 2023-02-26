using Microsoft.Extensions.FileProviders;
using OpenDocs.API.Data;
using OpenDocs.API.Services;

namespace OpenDocs.API.Configurations.Extensions
{
    public static class StaticFileExtension
    {
        public static IApplicationBuilder UseCustomStaticFiles(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            var setting = new SettingService(context).GetSettings().Result;

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(setting.StorageBasePath),
                RequestPath = new PathString("/docs")
            });


            return app;
        }
    }
}
