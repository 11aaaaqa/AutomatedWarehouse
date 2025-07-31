using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.DTOs.Measurement_unit;
using AutomatedWarehouse.Api.Infrastructure.Services.Guide_services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllArchivedAsync(bool isArchived)
            => Ok(await measurementUnitService.GetAllAsync(isArchived));

        [HttpDelete]
        [Route("Delete/{measurementUnitId}")]
        public async Task<IActionResult> DeleteAsync(Guid measurementUnitId)
        {
            try
            {
                await measurementUnitService.DeleteAsync(measurementUnitId);
            }
            catch (InvalidOperationException)
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
            catch (InvalidOperationException)
            {
                return Conflict("Measurement unit with current name already exists");
            }

            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] MeasurementUnit model)
        {
            try
            {
                await measurementUnitService.UpdateAsync(model);
            }
            catch (InvalidOperationException)
            {
                return Conflict("Measurement unit with current name already exists");
            }

            return Ok();
        }
    }
}
