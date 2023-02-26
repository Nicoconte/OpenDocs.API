namespace OpenDocs.API.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string path) 
            : base($"Cannot find swagger document in {path}") 
        { }
    }
}
