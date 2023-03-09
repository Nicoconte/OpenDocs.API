using OpenDocs.API.Models;

namespace OpenDocs.API.Contracts.Responses
{
    public class GetAllApplicationsResponse
    {
        public List<Applications> Applications { get; set; } = new List<Applications>();
    }
}
