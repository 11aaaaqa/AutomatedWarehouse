using AutomatedWarehouse.Api.Domain.Exceptions;
using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Resource_services
{
    public class ReceiptResourceService(ApplicationDbContext context) : IReceiptResourceService
    {
        public async Task<List<ReceiptResource>> GetByReceiptDocumentIdAsync(Guid receiptDocumentId)
            => await context.ReceiptResources.Where(x => x.ReceiptDocumentId == receiptDocumentId).ToListAsync();

        public async Task UpdateReceiptDocumentResourcesAsync(List<ReceiptResource> receiptResources, Guid receiptDocumentId)
        {
            if (receiptResources.Any(x => x.ReceiptDocumentId != receiptDocumentId))
                throw new ReceiptDocumentIdMatchException(
                    "Receipt resources collection cannot contain an entity which ReceiptDocumentId property doesn't match with input receipt document id");

            var oldReceiptDocumentResources = await context.ReceiptResources.Where(
                x => x.ReceiptDocumentId == receiptDocumentId).ToListAsync();

            if (oldReceiptDocumentResources.Count == 0)
            {
                if (receiptResources.Count > 0)
                {
                    await context.ReceiptResources.AddRangeAsync(receiptResources);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                var receiptResourcesToDelete = oldReceiptDocumentResources.Except(receiptResources).ToList();
                var receiptResourcesToAdd = receiptResources.Except(oldReceiptDocumentResources).ToList();

                context.ReceiptResources.RemoveRange(receiptResourcesToDelete);
                await context.ReceiptResources.AddRangeAsync(receiptResourcesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
