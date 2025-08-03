namespace AutomatedWarehouse.Api.Domain.Exceptions
{
    public class EntityIsInUsageException : Exception
    {
        public EntityIsInUsageException(string message) :base(message) { }
    }
}
