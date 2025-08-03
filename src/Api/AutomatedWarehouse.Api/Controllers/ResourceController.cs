using AutomatedWarehouse.Api.Domain.Exceptions;
using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.DTOs.Resource;
using AutomatedWarehouse.Api.Infrastructure.Services.Guide_services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            catch (EntityIsInUsageException ex)
            {
                return Conflict(ex.Message);
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
            catch (DbUpdateException)
            {
                return Conflict("Resource with current name already exists");
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateName/{resourceId}")]
        public async Task<IActionResult> UpdateNameAsync(Guid resourceId, string newName)
        {
            var resource = await resourceService.GetByIdAsync(resourceId);
            resource.Name = newName;

            try
            {
                await resourceService.UpdateAsync(resource);
            }
            catch (DbUpdateException)
            {
                return Conflict("Resource with current name already exists");
            }

            return Ok();
        }

        [HttpGet]
        [Route("SetIsArchived/{resourceId}")]
        public async Task<IActionResult> SetIsArchivedAsync(Guid resourceId, bool isArchived)
        {
            var resource = await resourceService.GetByIdAsync(resourceId);
            resource.IsArchived = isArchived;

            await resourceService.UpdateAsync(resource);

            return Ok();
        }
    }
}
