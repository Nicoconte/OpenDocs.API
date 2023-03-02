namespace OpenDocs.API.Exceptions
{
    public class InactiveEnvironmentException : Exception
    {
        public InactiveEnvironmentException(string env) :base($"{env} Environment is currently inactive. Contact an administrator") { }
    }
}
