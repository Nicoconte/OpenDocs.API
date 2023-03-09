using Microsoft.AspNetCore.Mvc;
using OpenDocs.API.Contracts.Requests;
using OpenDocs.API.Contracts.Responses;
using OpenDocs.API.Services;

namespace OpenDocs.API.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("environments")]
        public async Task<IActionResult> GetEnvironments() => Ok(new GetEnvironmentsResponse() { Environments = (await _settingService.GetEnvironments()).Where(e => e.IsActive).Select(e => e.EnvironmentType).ToList() });
        

        [HttpGet]
        public async Task<IActionResult> GetSettings() => Ok(new GetSettingsResponse() { Settings = await _settingService.GetSettings() }); 

        [HttpPost]
        public async Task<IActionResult> CreateInitialSettings() => Ok(await _settingService.InitSettings());
        
        [HttpPut("storage")]
        public async Task<IActionResult> UpdateStorage([FromBody] UpdateStoragePathRequest req) => Ok(await _settingService.UpdateStorageBasePath(req.Path));

        [HttpPut("cron")]
        public async Task<IActionResult> UpdateCron([FromBody] UpdateRetentionDaysRequest req) => Ok(await _settingService.UpdateRetentionDays(req.Days));
    }
}
