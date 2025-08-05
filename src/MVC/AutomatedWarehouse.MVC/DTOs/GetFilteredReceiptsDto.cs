namespace AutomatedWarehouse.MVC.DTOs
{
    public class GetFilteredReceiptsDto
    {
        public List<string> ReceiptNumbers { get; set; } = new();
        public List<Guid> ResourceIds { get; set; } = new();
        public List<Guid> MeasurementUnitIds { get; set; } = new();
    }
}
