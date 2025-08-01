using AutomatedWarehouse.Api.Domain.Models;

namespace AutomatedWarehouse.Api.DTOs.Receipt
{
    public class UpdateReceiptDocumentDto
    {
        public Guid ReceiptDocumentId { get; set; }
        public uint ReceiptNumber { get; set; }
        public DateOnly ReceiptDate { get; set; }
        public List<ReceiptResource> ReceiptResources { get; set; } = new();
    }
}
