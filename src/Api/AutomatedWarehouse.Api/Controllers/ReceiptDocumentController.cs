using AutomatedWarehouse.Api.Domain.Exceptions;
using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.DTOs.Receipt;
using AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Document_services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptDocumentController(IReceiptDocumentService receiptDocumentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateReceiptDocumentDto model)
        {
            try
            {
                await receiptDocumentService.AddAsync(new ReceiptDocument
                {
                    Id = model.ReceiptDocumentId,
                    ReceiptDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    ReceiptNumber = model.ReceiptNumber,
                    ReceiptResources = model.ReceiptResources
                });
            }
            catch (DbUpdateException)
            {
                return Conflict("Receipt document with current receipt number already exists");
            }
            catch (ReceiptDocumentIdMatchException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateReceiptDocumentDto model)
        {
            try
            {
                await receiptDocumentService.UpdateAsync(new ReceiptDocument
                {
                    Id = model.ReceiptDocumentId, ReceiptDate = model.ReceiptDate,
                    ReceiptNumber = model.ReceiptNumber, ReceiptResources = model.ReceiptResources
                });
            }
            catch (DbUpdateException)
            {
                return Conflict("Receipt document with current receipt number already exists");
            }
            catch (ReceiptDocumentIdMatchException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{receiptDocumentId}")]
        public async Task<IActionResult> DeleteAsync(Guid receiptDocumentId)
        {
            await receiptDocumentService.DeleteAsync(receiptDocumentId);

            return Ok();
        }

        [HttpPost]
        [Route("GetFilteredReceiptDocuments")]
        public async Task<IActionResult> GetFilteredReceiptDocumentsAsync([FromBody] GetFilteredReceiptDocumentsDto model)
        {
            var receiptDocuments = await receiptDocumentService.GetReceiptDocumentsAsync(
                model.DateFrom, model.DateUntil, model.ReceiptNumbers, model.ResourceIds, model.MeasurementUnitIds);
            return Ok(receiptDocuments);
        }

        [HttpGet]
        [Route("GetReceiptDocumentNumbers")]
        public async Task<IActionResult> GetReceiptDocumentNumbersAsync()
        {
            return Ok(await receiptDocumentService.GetReceiptDocumentNumbersAsync());
        }

        [HttpGet]
        [Route("{receiptDocumentId}")]
        public async Task<IActionResult> GetReceiptResourcesByDocumentAsync(Guid receiptDocumentId)
        {
            return Ok(await receiptDocumentService.GetByIdAsync(receiptDocumentId));
        }
    }
}
