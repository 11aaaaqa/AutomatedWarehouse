using AutomatedWarehouse.MVC.DTOs;
using AutomatedWarehouse.MVC.Response_models.Resource;

namespace AutomatedWarehouse.MVC.Models.View_models
{
    public class UpdateReceiptDocumentViewModel
    {
        public UpdateReceiptDto ReceiptDocument { get; set; }
        public List<ResourceResponseModel> AvailableResources { get; set; }
        public List<MeasurementUnitResponseModel> AvailableMeasurementUnits { get; set; }
    }
}
