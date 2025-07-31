namespace AutomatedWarehouse.Api.Domain.Exceptions
{
    public class ReceiptDocumentIdMatchException : Exception
    {
        public ReceiptDocumentIdMatchException(string message) : base(message)
        { }
    }
}
