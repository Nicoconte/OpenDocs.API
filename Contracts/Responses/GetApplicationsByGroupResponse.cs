using OpenDocs.API.Models;

namespace OpenDocs.API.Contracts.Responses
{
    public class GetApplicationsByGroupResponse
    {
        public List<Applications> Applications { get; set; } = new List<Applications>();
    }
}
