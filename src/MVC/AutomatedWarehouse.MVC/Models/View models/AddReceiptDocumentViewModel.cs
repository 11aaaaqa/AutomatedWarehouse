using AutomatedWarehouse.MVC.Response_models.Resource;

namespace AutomatedWarehouse.MVC.Models.View_models
{
    public class AddReceiptDocumentViewModel
    {
        public List<ResourceResponseModel> Resources { get; set; }
        public List<MeasurementUnitResponseModel> MeasurementUnits { get; set; }
    }
}
