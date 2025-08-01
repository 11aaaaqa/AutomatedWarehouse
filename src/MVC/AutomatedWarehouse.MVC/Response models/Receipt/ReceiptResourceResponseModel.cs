namespace AutomatedWarehouse.MVC.Response_models.Receipt
{
    public class ReceiptResourceResponseModel
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public Guid MeasurementUnitId { get; set; }
        public uint Quantity { get; set; }
        public Guid ReceiptDocumentId { get; set; }
    }
}
