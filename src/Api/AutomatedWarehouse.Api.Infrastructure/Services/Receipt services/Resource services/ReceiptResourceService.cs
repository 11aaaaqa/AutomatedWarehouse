using AutomatedWarehouse.Api.Domain.Exceptions;
using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Resource_services
{
    public class ReceiptResourceService(ApplicationDbContext context) : IReceiptResourceService
    {
        public async Task UpdateReceiptDocumentResourcesAsync(List<ReceiptResource> receiptResources, Guid receiptDocumentId)
        {
            if (receiptResources.Any(x => x.ReceiptDocumentId != receiptDocumentId))
                throw new ReceiptDocumentIdMatchException(
                    "Receipt resources collection cannot contain an entity which ReceiptDocumentId property doesn't match with input receipt document id");

            var currentReceiptDocumentResources = await context.ReceiptResources.Where(
                x => x.ReceiptDocumentId == receiptDocumentId).ToListAsync();

            if (currentReceiptDocumentResources.Count == 0)
            {
                if (receiptResources.Count > 0)
                {
                    await context.ReceiptResources.AddRangeAsync(receiptResources);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                foreach (var receiptResource in receiptResources)
                {
                    var currentReceiptResource =
                        currentReceiptDocumentResources.SingleOrDefault(x => x.Id == receiptResource.Id);
                    if (currentReceiptResource != null)
                    {
                        currentReceiptDocumentResources.Remove(currentReceiptResource);
                        currentReceiptResource.MeasurementUnitId = receiptResource.MeasurementUnitId;
                        currentReceiptResource.Quantity = receiptResource.Quantity;
                        currentReceiptResource.ResourceId = receiptResource.ResourceId;
                        context.ReceiptResources.Update(currentReceiptResource);
                    }
                    else
                    {
                        await context.ReceiptResources.AddAsync(receiptResource);
                    }
                }

                if (currentReceiptDocumentResources.Count > 0)
                    context.ReceiptResources.RemoveRange(currentReceiptDocumentResources);

                await context.SaveChangesAsync();
            }
        }
    }
}
