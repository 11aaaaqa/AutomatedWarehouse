namespace AutomatedWarehouse.Api.Domain.Models
{
    public class ReceiptDocument
    {
        public Guid Id { get; set; }
        public string ReceiptNumber { get; set; }
        public DateOnly ReceiptDate { get; set; }
        public List<ReceiptResource> ReceiptResources { get; set; } = new();
    }
}
