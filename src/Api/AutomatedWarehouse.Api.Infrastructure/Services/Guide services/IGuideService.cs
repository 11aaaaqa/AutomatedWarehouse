namespace AutomatedWarehouse.Api.Infrastructure.Services.Guide_services
{
    public interface IGuideService<T>
    {
        Task<T> GetByIdAsync(Guid entityId);
        Task<List<T>> GetAllAsync(bool isArchived);
        Task UpdateAsync(T model);
        Task CreateAsync(T model);
        Task DeleteAsync(Guid entityId);
    }
}
