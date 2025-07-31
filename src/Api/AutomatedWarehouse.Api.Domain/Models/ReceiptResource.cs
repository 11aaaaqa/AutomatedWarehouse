namespace AutomatedWarehouse.Api.Domain.Models
{
    public class ReceiptResource
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public Guid MeasurementUnitId { get; set; }
        public uint Quantity { get; set; }
        public Guid ReceiptDocumentId { get; set; }
    }
}
