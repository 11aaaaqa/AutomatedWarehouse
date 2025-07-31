using AutomatedWarehouse.Api.Domain.Models;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Resource_services
{
    public interface IReceiptResourceService
    {
        Task<List<ReceiptResource>> GetByReceiptDocumentIdAsync(Guid receiptDocumentId);
        Task UpdateReceiptDocumentResourcesAsync(List<ReceiptResource> receiptResources, Guid receiptDocumentId);
    }
}
