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
        public ReceiptController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            url = $"{configuration["Api:Url:Scheme"]}://{configuration["Api:Url:Domain"]}";
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
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("В системе уже зарегистрирована накладная с таким номером");
            }

            response.EnsureSuccessStatusCode();

            return RedirectToAction("GetReceiptDocuments");
        }

        [Route("receipts")]
        [HttpGet]
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

            return View(receiptDocuments);
        }
    }
}
