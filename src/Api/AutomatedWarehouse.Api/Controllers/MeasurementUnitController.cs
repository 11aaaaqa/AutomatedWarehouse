using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.DTOs.Measurement_unit;
using AutomatedWarehouse.Api.Infrastructure.Services.Guide_services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitController(IGuideService<MeasurementUnit> measurementUnitService) : ControllerBase
    {
        [HttpGet]
        [Route("GetById/{measurementUnitId}")]
        public async Task<IActionResult> GetByIdAsync(Guid measurementUnitId)
            => Ok(await measurementUnitService.GetByIdAsync(measurementUnitId));

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync(bool isArchived)
            => Ok(await measurementUnitService.GetAllAsync(isArchived));

        [HttpDelete]
        [Route("Delete/{measurementUnitId}")]
        public async Task<IActionResult> DeleteAsync(Guid measurementUnitId)
        {
            try
            {
                await measurementUnitService.DeleteAsync(measurementUnitId);
            }
            catch (DbUpdateException)
            {
                return Conflict("Measurement unit cannot be deleted because it is used in another table");
            }
            return Ok();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMeasurementUnitDto model)
        {
            try
            {
                await measurementUnitService.CreateAsync(new MeasurementUnit
                    { Id = Guid.NewGuid(), Name = model.Name });
            }
            catch (DbUpdateException)
            {
                return Conflict("Measurement unit with current name already exists");
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateName/{measurementUnitId}")]
        public async Task<IActionResult> UpdateNameAsync(Guid measurementUnitId, string newName)
        {
            var measurementUnit = await measurementUnitService.GetByIdAsync(measurementUnitId);
            measurementUnit.Name = newName;

            try
            {
                await measurementUnitService.UpdateAsync(measurementUnit);
            }
            catch (DbUpdateException)
            {
                return Conflict("Measurement unit with current name already exists");
            }

            return Ok();
        }

        [HttpGet]
        [Route("SetIsArchived/{measurementUnitId}")]
        public async Task<IActionResult> SetIsArchivedAsync(Guid measurementUnitId, bool isArchived)
        {
            var measurementUnit = await measurementUnitService.GetByIdAsync(measurementUnitId);
            measurementUnit.IsArchived = isArchived;

            await measurementUnitService.UpdateAsync(measurementUnit);

            return Ok();
        }
    }
}
