namespace OpenDocs.API.Exceptions
{
    public class FolderNotFoundException : Exception
    {
        public FolderNotFoundException(string path) : 
            base($"Cannot find the directory in {path}") 
        { }
    }
}
