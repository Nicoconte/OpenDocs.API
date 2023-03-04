using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenDocs.API.Contracts.Requests;
using OpenDocs.API.Exceptions;
using OpenDocs.API.Models;
using OpenDocs.API.Services;
using System.Text.RegularExpressions;

namespace OpenDocs.API.Controllers
{
    [Route("api/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly ISettingService _settingService;
        private readonly IApplicationService _applicationService;
    
        public ApplicationController(IStorageService storageService, ISettingService settingService, IApplicationService applicationService)
        {
            _storageService = storageService;
            _settingService = settingService;
            _applicationService = applicationService;
        }

        [HttpPost("sync-docs")]
        public async Task<IActionResult> SyncApplicationDocs([FromForm] SyncApplicationDocsRequest request)
        {
            try
            {
                await _settingService.CheckEnvironmentAccess(request.Environment, request.AccessKey);

                var setting = await _settingService.GetSettings();
                var app = await _applicationService.GetApplicationByName(request.ApplicationName);

                string appId = app is null ? Guid.NewGuid().ToString() : app.Id;

                string folderPath = string.Concat(setting.StorageBasePath, appId);
                string filePath = string.Concat(folderPath, "/", request.Environment, "/", $"{request.ApplicationName}_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.json");

                if (app is null)
                {
                    await _applicationService.CreateApplication(new Applications()
                    {
                        Id = appId,
                        Name = request.ApplicationName,
                        GroupId = request?.GroupID
                    });

                    _storageService.CreateFolder(setting.StorageBasePath, appId);

                    (await _settingService.GetEnvironments()).ForEach(env => _storageService.CreateFolder(folderPath, env.EnvironmentType));

                }

                if (string.IsNullOrWhiteSpace(app?.GroupId) || app?.GroupId != request.GroupID)
                {
                    await _applicationService.UpdateGroupId(request.ApplicationName, request?.GroupID);
                }

                await _applicationService.SetCurrentModificationDate(request.ApplicationName);

                _storageService.CreateFile(filePath, request.SwaggerFile.OpenReadStream());

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteApplication([FromRoute] string applicationName)
        {
            try
            {
                var deleted = await _applicationService.DeleteApplication(applicationName);

                return Ok(new
                {
                    Success = deleted
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

        [HttpGet("{applicationName}")]
        public async Task<IActionResult> GetApplicationByName([FromRoute] string applicationName)
        {
            try
            {
                var app = await _applicationService.GetApplicationByName(applicationName);

                if (app is null) throw new ApplicationNotFoundException(applicationName);

                var setting = await _settingService.GetSettings();

                var files = new List<object>();

                (await _settingService.GetEnvironments())
                    .Where(s => s.IsActive)
                    .Select(s => s.EnvironmentType)
                    .ToList()
                    .ForEach(env => files.Add(new
                    {
                        Files = _storageService.GetFilesFromFolder($"{setting.StorageBasePath}/{app.Id}/{env}/"),
                        Environment = env
                    }));

                return Ok(new
                {
                    Success = true,
                    Application = app,
                    ApplicationFiles = files
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

        [HttpGet]
        public async Task<IActionResult> GetAllApplications([FromQuery] int startIndex = 0, int quantity = 10, string? name = "")
        {
            try
            {
                var groups = await _applicationService.GetAllApplications(startIndex, quantity, name);

                return Ok(new
                {
                    Success = true,
                    Applications = groups
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

        [HttpGet("groups/{groupId}")]
        public async Task<IActionResult> GetApplicationsByGroup([FromRoute] string groupId)
        {
            try
            {
                var apps = await _applicationService.GetApplicationsByGroup(groupId);

                return Ok(new
                {
                    Success = true,
                    Groups = apps
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

        [HttpGet("groups")]
        public async Task<IActionResult> GetAllGroups([FromQuery] int startIndex = 0, int quantity = 10, string? name = "")
        {
            try
            {
                var groups = await _applicationService.GetAllGroups(startIndex, quantity, name);

                return Ok(new
                {
                    Success = true,
                    Groups = groups
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
