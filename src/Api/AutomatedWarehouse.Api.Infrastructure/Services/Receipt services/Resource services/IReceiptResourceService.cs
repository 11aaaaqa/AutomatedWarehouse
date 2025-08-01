using AutomatedWarehouse.Api.Domain.Models;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Resource_services
{
    public interface IReceiptResourceService
    {
        Task UpdateReceiptDocumentResourcesAsync(List<ReceiptResource> receiptResources, Guid receiptDocumentId);
    }
}
