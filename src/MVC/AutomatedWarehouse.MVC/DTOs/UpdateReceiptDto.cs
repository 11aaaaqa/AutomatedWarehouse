using AutomatedWarehouse.MVC.Response_models.Receipt;

namespace AutomatedWarehouse.MVC.DTOs
{
    public class UpdateReceiptDto
    {
        public Guid ReceiptDocumentId { get; set; }
        public string ReceiptNumber { get; set; }
        public DateOnly ReceiptDate { get; set; }
        public List<ReceiptResourceResponseModel> ReceiptResources { get; set; } = new();
    }
}
