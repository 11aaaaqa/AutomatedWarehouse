using System.Net;
using System.Text;
using System.Text.Json;
using AutomatedWarehouse.MVC.DTOs;
using AutomatedWarehouse.MVC.Models.View_models;
using AutomatedWarehouse.MVC.Response_models.Receipt;
using AutomatedWarehouse.MVC.Response_models.Resource;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedWarehouse.MVC.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string url;
        private readonly ILogger<ReceiptController> logger;
        public ReceiptController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ReceiptController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            url = $"{configuration["Api:Url:Scheme"]}://{configuration["Api:Url:Domain"]}";
            this.logger = logger;
        }

        [HttpGet]
        [Route("receipts/add")]
        public async Task<IActionResult> AddReceipt()
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var resourcesResponse = await httpClient.GetAsync($"{url}/api/Resource/GetAll?isArchived=false");
            resourcesResponse.EnsureSuccessStatusCode();
            var resources = await resourcesResponse.Content.ReadFromJsonAsync<List<ResourceResponseModel>>();

            var measurementUnitsResponse = await httpClient.GetAsync($"{url}/api/MeasurementUnit/GetAll?isArchived=false");
            measurementUnitsResponse.EnsureSuccessStatusCode();
            var measurementUnits = await measurementUnitsResponse.Content.ReadFromJsonAsync<List<MeasurementUnitResponseModel>>();

            return View(new AddReceiptDocumentViewModel{MeasurementUnits = measurementUnits, Resources = resources});
        }

        [HttpPost]
        [Route("receipts/add")]
        public async Task<IActionResult> AddReceipt([FromBody]AddReceiptDto model)
        {
            Guid receiptDocumentId = Guid.NewGuid();
            List<ReceiptResourceResponseModel> receiptResources = new();
            foreach (var resource in model.ReceiptResources)
            {
                receiptResources.Add(new ReceiptResourceResponseModel
                {
                    Id = Guid.NewGuid(), ReceiptDocumentId = receiptDocumentId, MeasurementUnitId = resource.MeasurementUnitId,
                    Quantity = resource.Quantity, ResourceId = resource.ResourceId
                });
            }

            using HttpClient httpClient = httpClientFactory.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                ReceiptDocumentId = receiptDocumentId,
                ReceiptNumber = model.Number,
                ReceiptDate = model.Date,
                ReceiptResources = receiptResources
            }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                $"{url}/api/ReceiptDocument", jsonContent);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    return Conflict("В системе уже зарегистрирована накладная с таким номером");
                case HttpStatusCode.BadRequest:
                    logger.LogCritical("Could not add receipt because Receipt Document ID and Receipt Resources Document IDs were different");
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            response.EnsureSuccessStatusCode();

            return RedirectToAction("GetReceiptDocuments");
        }

        [HttpGet]
        [Route("receipts")]
        public async Task<IActionResult> GetReceiptDocuments()
        {
            DateOnly dateFrom = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7);
            DateOnly dateUntil = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(7);
            ViewBag.DateUntil = dateUntil.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dateFrom.ToString("yyyy-MM-dd");

            using HttpClient httpClient = httpClientFactory.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(new {DateFrom = dateFrom, DateUntil = dateUntil}),
                Encoding.UTF8, "application/json");

            var receiptDocumentsResponse = await httpClient.PostAsync($"{url}/api/ReceiptDocument/GetFilteredReceiptDocuments", jsonContent);
            receiptDocumentsResponse.EnsureSuccessStatusCode();
            var receiptDocuments = await receiptDocumentsResponse.Content.ReadFromJsonAsync<List<ReceiptDocumentResponseModel>>();

            var receiptNumbersResponse = await httpClient.GetAsync($"{url}/api/ReceiptDocument/GetReceiptDocumentNumbers");
            receiptNumbersResponse.EnsureSuccessStatusCode();
            List<string> receiptNumbers = await receiptNumbersResponse.Content.ReadFromJsonAsync<List<string>>();

            var availableMeasurementUnitsResponse = await httpClient.GetAsync($"{url}/api/MeasurementUnit/GetAll?isArchived={false}");
            availableMeasurementUnitsResponse.EnsureSuccessStatusCode();
            var availableMeasurementUnits = await availableMeasurementUnitsResponse.Content.ReadFromJsonAsync<List<MeasurementUnitResponseModel>>();

            var availableResourcesResponse = await httpClient.GetAsync($"{url}/api/Resource/GetAll?isArchived={false}");
            availableResourcesResponse.EnsureSuccessStatusCode();
            var availableResources = await availableResourcesResponse.Content.ReadFromJsonAsync<List<ResourceResponseModel>>();

            return View(new GetReceiptDocumentsViewModel
            {
                AvailableMeasurementUnits = availableMeasurementUnits, AvailableResources = availableResources,
                ReceiptDocuments = receiptDocuments, ReceiptNumbers = receiptNumbers
            });
        }

        [HttpPost]
        [Route("receipts/filter/get-json")]
        public async Task<IActionResult> GetFilteredReceiptsJson([FromBody] GetFilteredReceiptsDto model)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var filteredReceiptsResponse = await httpClient.PostAsync(
                $"{url}/api/ReceiptDocument/GetFilteredReceiptDocuments", jsonContent);
            filteredReceiptsResponse.EnsureSuccessStatusCode();
            var filteredReceipts = await filteredReceiptsResponse.Content.ReadFromJsonAsync<List<ReceiptDocumentResponseModel>>();

            return new JsonResult(filteredReceipts);
        }

        [HttpGet]
        [Route("receipts/{receiptId}/update")]
        public async Task<IActionResult> UpdateReceipt(Guid receiptId)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var receiptDocumentResponse = await httpClient.GetAsync($"{url}/api/ReceiptDocument/{receiptId}");
            receiptDocumentResponse.EnsureSuccessStatusCode();
            var receiptDocument = await receiptDocumentResponse.Content.ReadFromJsonAsync<ReceiptDocumentResponseModel>();

            var resourcesResponse = await httpClient.GetAsync($"{url}/api/Resource/GetAll?isArchived=false");
            resourcesResponse.EnsureSuccessStatusCode();
            var resources = await resourcesResponse.Content.ReadFromJsonAsync<List<ResourceResponseModel>>();

            var measurementUnitsResponse = await httpClient.GetAsync($"{url}/api/MeasurementUnit/GetAll?isArchived=false");
            measurementUnitsResponse.EnsureSuccessStatusCode();
            var measurementUnits = await measurementUnitsResponse.Content.ReadFromJsonAsync<List<MeasurementUnitResponseModel>>();

            return View(new UpdateReceiptDocumentViewModel
            {
                AvailableMeasurementUnits = measurementUnits, AvailableResources = resources,
                ReceiptDocument = new UpdateReceiptDto
                {
                    ReceiptResources = receiptDocument.ReceiptResources, ReceiptDate = receiptDocument.ReceiptDate,
                    ReceiptDocumentId = receiptDocument.Id, ReceiptNumber = receiptDocument.ReceiptNumber
                }
            });
        }

        [HttpPost]
        [Route("receipts/{receiptId}/update")]
        public async Task<IActionResult> UpdateReceipt([FromBody] UpdateReceiptDto model)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var updateReceiptResponse = await httpClient.PutAsync($"{url}/api/ReceiptDocument", jsonContent);

            switch (updateReceiptResponse.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    return Conflict("В системе уже зарегистрирована накладная с таким номером");
                case HttpStatusCode.BadRequest:
                    logger.LogCritical("Could not update receipt because Receipt Document ID and Receipt Resources Document IDs were different");
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            updateReceiptResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetReceiptDocuments");
        }

        [HttpPost]
        [Route("receipts/{receiptDocumentId}/delete")]
        public async Task<IActionResult> DeleteReceiptDocument([FromRoute] Guid receiptDocumentId)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var deleteResponse = await httpClient.DeleteAsync($"{url}/api/ReceiptDocument/{receiptDocumentId}");
            deleteResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetReceiptDocuments");
        }
    }
}
