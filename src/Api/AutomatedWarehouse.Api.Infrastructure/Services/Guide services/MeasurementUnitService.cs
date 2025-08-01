using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Guide_services
{
    public class MeasurementUnitService(ApplicationDbContext context) : IGuideService<MeasurementUnit>
    {
        public async Task<MeasurementUnit> GetByIdAsync(Guid entityId)
            => await context.MeasurementUnits.SingleAsync(x => x.Id == entityId);

        public async Task<List<MeasurementUnit>> GetAllAsync(bool isArchived)
            => await context.MeasurementUnits.Where(x => x.IsArchived == isArchived).ToListAsync();

        public async Task UpdateAsync(MeasurementUnit model)
        {
            context.MeasurementUnits.Update(model);
            await context.SaveChangesAsync();
        }

        public async Task CreateAsync(MeasurementUnit model)
        {
            await context.MeasurementUnits.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid entityId) //check
        {
            var measurementUnit = await context.MeasurementUnits.SingleAsync(x => x.Id == entityId);
            context.MeasurementUnits.Remove(measurementUnit);
            await context.SaveChangesAsync();
        }
    }
}
