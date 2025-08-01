using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Document_services
{
    public class ReceiptDocumentService(ApplicationDbContext context) : IReceiptDocumentService
    {
        public async Task<ReceiptDocument> GetByIdAsync(Guid receiptDocumentId)
            => await context.ReceiptDocuments.Include(x => x.ReceiptResources)
                .SingleAsync(x => x.Id == receiptDocumentId);

        public async Task<List<ReceiptDocument>> GetReceiptDocumentsAsync(DateOnly dateFrom, DateOnly dateUntil,
            List<uint> receiptNumbers, List<Guid> resourceIds, List<Guid> measurementUnitIds)
        {
            var receiptDocuments = context.ReceiptDocuments
                .Where(x => x.ReceiptDate > dateFrom && x.ReceiptDate < dateUntil).AsQueryable();

            if (receiptNumbers.Count > 0)
                receiptDocuments = receiptDocuments.Where(x => receiptNumbers.Contains(x.ReceiptNumber)).AsQueryable();

            if (resourceIds.Count > 0 && measurementUnitIds.Count > 0)
            {
                receiptDocuments = receiptDocuments.Where(
                        x => x.ReceiptResources.Any(y =>
                                resourceIds.Contains(y.ResourceId) && measurementUnitIds.Contains(y.MeasurementUnitId)))
                    .AsQueryable();
            }
            else if (resourceIds.Count > 0 && measurementUnitIds.Count == 0)
            {
                receiptDocuments = receiptDocuments.Where(x => x.ReceiptResources.Any(
                    y => resourceIds.Contains(y.ResourceId))).AsQueryable();
            }
            else if (resourceIds.Count == 0 && measurementUnitIds.Count > 0)
            {
                receiptDocuments = receiptDocuments.Where(x => x.ReceiptResources.Any(
                    y => measurementUnitIds.Contains(y.MeasurementUnitId))).AsQueryable();
            }

            return await receiptDocuments.Include(x => x.ReceiptResources).ToListAsync();
        }

        public async Task<List<uint>> GetReceiptDocumentNumbersAsync()
        {
            return await context.ReceiptDocuments.Select(x => x.ReceiptNumber).ToListAsync();
        }

        public async Task AddAsync(ReceiptDocument model)
        {
            await context.ReceiptDocuments.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid receiptDocumentId)
        {
            var receiptDocument = await context.ReceiptDocuments.SingleOrDefaultAsync(x => x.Id == receiptDocumentId);
            if (receiptDocument != null)
            {
                context.ReceiptDocuments.Remove(receiptDocument);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(ReceiptDocument model)
        {
            context.ReceiptDocuments.Update(model);
            await context.SaveChangesAsync();
        }
    }
}
