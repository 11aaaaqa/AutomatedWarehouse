using System.Net;
using System.Text;
using System.Text.Json;
using AutomatedWarehouse.MVC.Response_models.Resource;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedWarehouse.MVC.Controllers
{
    public class MeasurementUnitController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string url;
        public MeasurementUnitController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            url = $"{configuration["Api:Url:Scheme"]}://{configuration["Api:Url:Domain"]}";
        }

        [HttpGet]
        [Route("measurement-units")]
        public async Task<IActionResult> GetMeasurementUnits(bool isArchived = false)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var measurementUnitsResponse = await httpClient.GetAsync($"{url}/api/MeasurementUnit/GetAll?isArchived={isArchived}");
            measurementUnitsResponse.EnsureSuccessStatusCode();
            var measurementUnits = await measurementUnitsResponse.Content.ReadFromJsonAsync<List<MeasurementUnitResponseModel>>();
            ViewBag.IsArchived = isArchived;

            return View(measurementUnits);
        }

        [HttpGet]
        [Route("measurement-units/add")]
        public async Task<IActionResult> AddMeasurementUnit()
        {
            return View();
        }

        [HttpPost]
        [Route("measurement-units/add")]
        public async Task<IActionResult> AddMeasurementUnit(string measurementUnitName)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                Name = measurementUnitName
            }), Encoding.UTF8, "application/json");

            var addMeasurementUnitResponse = await httpClient.PostAsync($"{url}/api/MeasurementUnit/Create", jsonContent);
            if (addMeasurementUnitResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("Единица измерения с таким наименованиием уже существует");
            }

            addMeasurementUnitResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetMeasurementUnits");
        }

        [HttpGet]
        [Route("measurement-units/{measurementUnitId}")]
        public async Task<IActionResult> GetMeasurementUnit(Guid measurementUnitId)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var measurementUnitResponse = await httpClient.GetAsync($"{url}/api/MeasurementUnit/GetById/{measurementUnitId}");
            measurementUnitResponse.EnsureSuccessStatusCode();
            var measurementUnit = await measurementUnitResponse.Content.ReadFromJsonAsync<MeasurementUnitResponseModel>();

            return View(measurementUnit);
        }

        [HttpPost]
        [Route("measurement-units/{measurementUnitId}/delete")]
        public async Task<IActionResult> DeleteMeasurementUnit(Guid measurementUnitId)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var deleteMeasurementUnitResponse = await httpClient.DeleteAsync($"{url}/api/MeasurementUnit/Delete/{measurementUnitId}");
            if (deleteMeasurementUnitResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("Невозможно удалить ресурс, так как он используется");
            }

            deleteMeasurementUnitResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetMeasurementUnits");
        }

        [HttpPost]
        [Route("measurement-units/{measurementUnitId}/update")]
        public async Task<IActionResult> UpdateMeasurementUnitName(Guid measurementUnitId, string newName)
        {
            using HttpClient httpsClient = httpClientFactory.CreateClient();
            
            var updateNameResponse = await httpsClient.GetAsync(
                $"{url}/api/MeasurementUnit/UpdateName/{measurementUnitId}?newName={newName}");
            if (updateNameResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("Единица измерения с таким наименованием уже существует");
            }

            updateNameResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetMeasurementUnits");
        }

        [HttpPost]
        [Route("measurement-units/{measurementUnitId}/set-is-archived")]
        public async Task<IActionResult> SetIsArchived(Guid measurementUnitId, bool isArchived)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var setIsArchivedResponse = await httpClient.GetAsync(
                $"{url}/api/MeasurementUnit/SetIsArchived/{measurementUnitId}?isArchived={isArchived}");
            setIsArchivedResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetMeasurementUnits");
        }
    }
}
