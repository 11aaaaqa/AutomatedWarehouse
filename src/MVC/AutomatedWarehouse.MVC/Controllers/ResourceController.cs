using AutomatedWarehouse.MVC.Response_models.Resource;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text;

namespace AutomatedWarehouse.MVC.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string url;
        public ResourceController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            url = $"{configuration["Api:Url:Scheme"]}://{configuration["Api:Url:Domain"]}";
        }

        [HttpGet]
        [Route("resources")]
        public async Task<IActionResult> GetResources(bool isArchived = false)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var resourcesResponse = await httpClient.GetAsync($"{url}/api/Resource/GetAll?isArchived={isArchived}");
            resourcesResponse.EnsureSuccessStatusCode();
            var resources = await resourcesResponse.Content.ReadFromJsonAsync<List<ResourceResponseModel>>();
            ViewBag.IsArchived = isArchived;

            return View(resources);
        }

        [HttpGet]
        [Route("resources/add")]
        public IActionResult AddResource()
        {
            return View();
        }

        [HttpPost]
        [Route("resources/add")]
        public async Task<IActionResult> AddResource(string resourceName)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();
            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                Name = resourceName
            }), Encoding.UTF8, "application/json");

            var addResourceResponse = await httpClient.PostAsync($"{url}/api/Resource/Create", jsonContent);
            if (addResourceResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("В системе уже зарегистрирован ресурс с таким наименованием");
            }

            addResourceResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetResources");
        }

        [HttpGet]
        [Route("resources/{resourceId}")]
        public async Task<IActionResult> GetResource(Guid resourceId)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var resourceResponse = await httpClient.GetAsync($"{url}/api/Resource/GetById/{resourceId}");
            resourceResponse.EnsureSuccessStatusCode();
            var resource = await resourceResponse.Content.ReadFromJsonAsync<ResourceResponseModel>();

            return View(resource);
        }

        [HttpPost]
        [Route("resources/{resourceId}/delete")]
        public async Task<IActionResult> DeleteResource(Guid resourceId)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var deleteResourcesResponse = await httpClient.DeleteAsync($"{url}/api/Resource/Delete/{resourceId}");
            if (deleteResourcesResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("Невозможно удалить ресурс, так как он используется");
            }

            deleteResourcesResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetResources");
        }

        [HttpPost]
        [Route("resources/{resourceId}/update")]
        public async Task<IActionResult> UpdateResourceName(Guid resourceId, string newName)
        {
            using HttpClient httpsClient = httpClientFactory.CreateClient();

            var updateNameResponse = await httpsClient.GetAsync(
                $"{url}/api/Resource/UpdateName/{resourceId}?newName={newName}");
            if (updateNameResponse.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("В системе уже зарегистрирован ресурс с таким наименованием");
            }

            updateNameResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetResources");
        }

        [HttpPost]
        [Route("resources/{resourceId}/set-is-archived")]
        public async Task<IActionResult> SetIsArchived(Guid resourceId, bool isArchived)
        {
            using HttpClient httpClient = httpClientFactory.CreateClient();

            var setIsArchivedResponse = await httpClient.GetAsync(
                $"{url}/api/Resource/SetIsArchived/{resourceId}?isArchived={isArchived}");
            setIsArchivedResponse.EnsureSuccessStatusCode();

            return RedirectToAction("GetResources");
        }
    }
}
