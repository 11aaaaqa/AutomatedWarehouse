using AutomatedWarehouse.Api.Domain.Models;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Document_services
{
    public interface IReceiptDocumentService
    {
        Task<ReceiptDocument> GetByIdAsync(Guid receiptDocumentId);
        Task<List<ReceiptDocument>> GetReceiptDocumentsAsync(DateOnly dateFrom, DateOnly dateUntil,
            List<string> receiptNumbers, List<Guid> resourceIds, List<Guid> measurementUnitIds);

        Task<List<string>> GetReceiptDocumentNumbersAsync();
        Task AddAsync(ReceiptDocument model);
        Task DeleteAsync(Guid receiptDocumentId);
        Task UpdateAsync(ReceiptDocument model);
    }
}
