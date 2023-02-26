namespace OpenDocs.API.Contracts.Requests
{
    public class SyncApplicationDocsRequest
    {
        public string Environment { get; set; }
        public string ApplicationName { get; set; }
        public string? AccessKey { get; set; }
        public string? GroupID { get; set; }
        public IFormFile SwaggerFile { get; set; }
    }
}
