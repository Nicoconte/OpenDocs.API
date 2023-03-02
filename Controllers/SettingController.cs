using Microsoft.AspNetCore.Mvc;
using OpenDocs.API.Contracts.Requests;
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
        public async Task<IActionResult> GetEnvironments()
        {
            try
            {
                return Ok((await _settingService.GetEnvironments()).Where(e => e.IsActive).Select(e => e.EnvironmentType));
            }
            catch(Exception ex) 
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings() 
        {
            try
            {
                return Ok(new
                {
                    Success = true,
                    Content = await _settingService.GetSettings()
                });
            }
            catch(Exception ex) 
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInitialSettings()
        {
            try
            {
                var isInitialized = await _settingService.InitSettings();

                return Ok(new
                {
                    Success = isInitialized
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPut("storage")]
        public async Task<IActionResult> UpdateStorage([FromBody] UpdateStoragePathRequest req)
        {
            try
            {
                var updated = await _settingService.UpdateStorageBasePath(req.Path);

                return Ok(new
                {
                    Success = updated
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

        }

        [HttpPut("cron")]
        public async Task<IActionResult> UpdateCron([FromBody] UpdateRetentionDaysRequest req)
        {
            try
            {
                var updated = await _settingService.UpdateRetentionDays(req.Days);

                return Ok(new
                {
                    Success = updated
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
