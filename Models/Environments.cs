namespace OpenDocs.API.Models
{
    public class Environments : BaseModel
    {
        public string EnvironmentType { get; set; }
        public string AccessKey { get; set; }
        public bool IsActive { get; set; }
    }

}
