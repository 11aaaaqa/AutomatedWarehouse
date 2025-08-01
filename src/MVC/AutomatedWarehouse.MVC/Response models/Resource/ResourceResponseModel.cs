namespace AutomatedWarehouse.MVC.Response_models.Resource
{
    public class ResourceResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
