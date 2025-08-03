namespace AutomatedWarehouse.MVC.DTOs
{
    public class AddReceiptReceiptResourceDto
    {
        public Guid ResourceId { get; set; }
        public Guid MeasurementUnitId { get; set; }
        public uint Quantity { get; set; }
    }
}
