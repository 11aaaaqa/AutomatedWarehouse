namespace AutomatedWarehouse.MVC.Response_models.Receipt
{
    public class ReceiptDocumentResponseModel
    {
        public Guid Id { get; set; }
        public string ReceiptNumber { get; set; }
        public DateOnly ReceiptDate { get; set; }
        public List<ReceiptResourceResponseModel> ReceiptResources { get; set; } = new();
    }
}
