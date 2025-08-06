using AutomatedWarehouse.Api.Domain.Exceptions;
using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Document_services
{
    public class ReceiptDocumentService(ApplicationDbContext context) : IReceiptDocumentService
    {
        public async Task<ReceiptDocument> GetByIdAsync(Guid receiptDocumentId)
        {
            return await context.ReceiptDocuments
                .Include(x => x.ReceiptResources)
                .ThenInclude(x => x.MeasurementUnit)
                .Include(x => x.ReceiptResources)
                .ThenInclude(x => x.Resource)
                .SingleAsync(x => x.Id == receiptDocumentId);
        }

        public async Task<List<ReceiptDocument>> GetReceiptDocumentsAsync(DateOnly dateFrom, DateOnly dateUntil,
            List<string> receiptNumbers, List<Guid> resourceIds, List<Guid> measurementUnitIds)
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

            return await receiptDocuments
                .Include(x => x.ReceiptResources)
                .ThenInclude(x => x.MeasurementUnit)
                .Include(x => x.ReceiptResources)
                .ThenInclude(x => x.Resource)
                .ToListAsync();
        }

        public async Task<List<string>> GetReceiptDocumentNumbersAsync()
        {
            return await context.ReceiptDocuments.Select(x => x.ReceiptNumber).ToListAsync();
        }

        public async Task AddAsync(ReceiptDocument model)
        {
            if (model.ReceiptResources.Any(x => x.ReceiptDocumentId != model.Id))
                throw new ReceiptDocumentIdMatchException(
                    "Receipt resources collection cannot contain an entity which ReceiptDocumentId property doesn't match with input receipt document id");

            await context.ReceiptDocuments.AddAsync(model);
            if (model.ReceiptResources.Count > 0)
                await context.ReceiptResources.AddRangeAsync(model.ReceiptResources);
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
            if (model.ReceiptResources.Any(x => x.ReceiptDocumentId != model.Id))
                throw new ReceiptDocumentIdMatchException(
                    "Receipt resources collection cannot contain an entity which ReceiptDocumentId property doesn't match with input receipt document id");

            context.ReceiptDocuments.Update(new ReceiptDocument{Id = model.Id, ReceiptDate = model.ReceiptDate, ReceiptNumber = model.ReceiptNumber});

            var currentReceiptDocumentResources = await context.ReceiptResources.Where(
                x => x.ReceiptDocumentId == model.Id).ToListAsync();

            if (currentReceiptDocumentResources.Count == 0)
            {
                if (model.ReceiptResources.Count > 0)
                {
                    await context.ReceiptResources.AddRangeAsync(model.ReceiptResources);
                }
            }
            else
            {
                foreach (var receiptResource in model.ReceiptResources)
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
            }

            await context.SaveChangesAsync();
        }
    }
}
