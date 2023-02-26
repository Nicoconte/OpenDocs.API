namespace OpenDocs.API.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName) 
            : base($"Entity {entityName} not found.") 
        { }
    }
}
