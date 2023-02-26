namespace OpenDocs.API.Models
{
    public class Settings : BaseModel
    {
        public string StorageBasePath { get; set; }
        public int RetentionDays { get; set; }
    }
}
