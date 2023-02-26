namespace OpenDocs.API.Exceptions
{
    public class ApplicationNotFoundException : Exception
    {
        public ApplicationNotFoundException(string appname) 
            : base($"Cannot find an application with name '{appname}'") 
        { }
    }
}
