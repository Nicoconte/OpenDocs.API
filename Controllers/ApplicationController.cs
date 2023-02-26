using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenDocs.API.Contracts.Requests;
using OpenDocs.API.Exceptions;
using OpenDocs.API.Models;
using OpenDocs.API.Services;
using System.Text.RegularExpressions;

namespace OpenDocs.API.Controllers
{
    [Route("api/application")]
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

                    _settingService.GetEnviroments().ForEach(env => _storageService.CreateFolder(folderPath, env));

                }

                if (string.IsNullOrWhiteSpace(app?.GroupId) || app?.GroupId != request.GroupID)
                {
                    await _applicationService.UpdateGroupId(request.ApplicationName, request?.GroupID);
                }

                _storageService.CreateFile(filePath, request.SwaggerFile.OpenReadStream());

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApplications([FromQuery] string? groupId)
        {
            try
            {
                var apps = await _applicationService.ListApplications(groupId);

                return Ok(new
                {
                    Success = true,
                    Content = apps
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

        [HttpGet("{name}")]
        public async Task<IActionResult> GetApplication([FromRoute] string name)
        {
            try
            {
                var app = await _applicationService.GetApplicationByName(name);

                if (app is null)
                    throw new ApplicationNotFoundException(name);


                var setting = await _settingService.GetSettings();

                List<object> filesPerEnv = new List<object>();

                _settingService.GetEnviroments().ForEach(env => filesPerEnv.Add(new
                {
                    Environment = env,
                    Files = _storageService.GetFilesFromFolder($"{setting.StorageBasePath}{app.Id}/{env}/")
                }));


                return Ok(new
                {
                    Success = true,
                    Content = new
                    {
                        app,
                        FilesPerEnvironment =  filesPerEnv
                    }
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
