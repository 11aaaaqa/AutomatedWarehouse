using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AutomatedWarehouse.Api.Infrastructure.Services.Guide_services
{
    public class ResourceService(ApplicationDbContext context) : IGuideService<Resource>
    {
        public async Task<Resource> GetByIdAsync(Guid entityId)
            => await context.Resources.SingleAsync(x => x.Id == entityId);

        public async Task<List<Resource>> GetAllArchivedAsync()
            => await context.Resources.Where(x => x.IsArchived).ToListAsync();

        public async Task<List<Resource>> GetAllUnarchivedAsync()
            => await context.Resources.Where(x => !x.IsArchived).ToListAsync();

        public async Task UpdateAsync(Resource model)
        {
            context.Resources.Update(model);
            await context.SaveChangesAsync();
        }

        public async Task CreateAsync(Resource model)
        {
            await context.Resources.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid entityId)
        {
            var resource = await context.Resources.SingleAsync(x => x.Id == entityId);
            context.Resources.Remove(resource);
            await context.SaveChangesAsync();
        }
    }
}
