using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenDocs.API.Contracts.Requests;
using OpenDocs.API.Contracts.Responses;
using OpenDocs.API.Exceptions;
using OpenDocs.API.Models;
using OpenDocs.API.Services;
using System.Net;
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
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteApplication([FromRoute] string applicationName)
        {
            var deleted = await _applicationService.DeleteApplication(applicationName);

            return Ok();
        }

        [HttpGet("{applicationName}")]
        public async Task<IActionResult> GetApplicationByName([FromRoute] string applicationName)
        {
            var application = await _applicationService.GetApplicationByName(applicationName);

            if (application is null) throw new ApplicationNotFoundException(applicationName);

            var setting = await _settingService.GetSettings();

            var applicationContent = new List<object>();

            (await _settingService.GetEnvironments())
                .Where(s => s.IsActive)
                .Select(s => s.EnvironmentType)
                .ToList()
                .ForEach(env => applicationContent.Add(new
                {
                    Environment = env,
                    Files = _storageService.GetFilesFromFolder($"{setting.StorageBasePath}/{application.Id}/{env}/"),
                }));

            return Ok(new GetApplicationByNameResponse()
            {
                Name = application.Name,
                GroupId = application.GroupId,
                Content = applicationContent
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApplications([FromQuery] int startIndex = 0, int quantity = 10, string? name = "")
        {
            var applications = await _applicationService.GetAllApplications(startIndex, quantity, name);

            return Ok(new GetAllApplicationsResponse()
            {
                Applications = applications
            });
        }

        [HttpGet("groups/{groupId}")]
        public async Task<IActionResult> GetApplicationsByGroup([FromRoute] string groupId)
        {
            var applications = await _applicationService.GetApplicationsByGroup(groupId);

            return Ok(new GetApplicationsByGroupResponse()
            {
                Applications = applications
            });
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetAllGroups([FromQuery] int startIndex = 0, int quantity = 10, string? name = "")
        {
            var groups = await _applicationService.GetAllGroups(startIndex, quantity, name);

            return Ok(new GetAllGroupsResponse()
            {
                Groups = groups
            });
        }
    }
}
