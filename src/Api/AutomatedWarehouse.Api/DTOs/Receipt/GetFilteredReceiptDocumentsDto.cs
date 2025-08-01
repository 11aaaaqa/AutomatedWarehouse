namespace AutomatedWarehouse.Api.DTOs.Receipt
{
    public class GetFilteredReceiptDocumentsDto
    {
        public DateOnly DateFrom { get; set; }
        public DateOnly DateUntil { get; set; }
        public List<uint> ReceiptNumbers { get; set; } = new();
        public List<Guid> ResourceIds { get; set; } = new();
        public List<Guid> MeasurementUnitIds { get; set; } = new();
    }
}
