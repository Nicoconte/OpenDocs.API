namespace OpenDocs.API.Contracts.Responses
{
    public class GetApplicationByNameResponse
    {
        public string Name { get; set; }
        public string? GroupId { get; set; }
        public List<object> Content { get; set; } = new List<object>();
    }
}
