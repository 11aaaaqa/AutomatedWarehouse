using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.DTOs.Resource;
using AutomatedWarehouse.Api.Infrastructure.Services.Guide_services;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedWarehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController(IGuideService<Resource> resourceService) : ControllerBase
    {
        [HttpGet]
        [Route("GetById/{resourceId}")]
        public async Task<IActionResult> GetByIdAsync(Guid resourceId)
            => Ok(await resourceService.GetByIdAsync(resourceId));

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync(bool isArchived)
            => Ok(await resourceService.GetAllAsync(isArchived));

        [HttpDelete]
        [Route("Delete/{resourceId}")]
        public async Task<IActionResult> DeleteAsync(Guid resourceId)
        {
            try
            {
                await resourceService.DeleteAsync(resourceId);
            }
            catch (InvalidOperationException)
            {
                return Conflict("Resource cannot be deleted because it is used in another table");
            }
            return Ok();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateResourceDto model)
        {
            try
            {
                await resourceService.CreateAsync(new Resource
                    { Id = Guid.NewGuid(), Name = model.Name });
            }
            catch (InvalidOperationException)
            {
                return Conflict("Resource with current name already exists");
            }

            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] Resource model)
        {
            try
            {
                await resourceService.UpdateAsync(model);
            }
            catch (InvalidOperationException)
            {
                return Conflict("Resource with current name already exists");
            }

            return Ok();
        }
    }
}
