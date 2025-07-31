namespace AutomatedWarehouse.Api.Domain.Models
{
    public class Resource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
