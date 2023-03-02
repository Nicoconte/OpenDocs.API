namespace OpenDocs.API.Exceptions
{
    public class InvalidEnvironmentAccessException : Exception
    {
        public InvalidEnvironmentAccessException(string env) 
            : base($"Cannot access to {env} Environment. Invalid access key") 
        { }
    }
}
