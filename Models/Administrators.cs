namespace OpenDocs.API.Models
{
    public class Administrators : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
