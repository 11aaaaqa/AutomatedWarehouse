using AutomatedWarehouse.MVC.Response_models.Receipt;
using AutomatedWarehouse.MVC.Response_models.Resource;

namespace AutomatedWarehouse.MVC.Models.View_models
{
    public class GetReceiptDocumentsViewModel
    {
        public List<ReceiptDocumentResponseModel> ReceiptDocuments { get; set; }
        public List<string> ReceiptNumbers { get; set; }
        public List<MeasurementUnitResponseModel> AvailableMeasurementUnits { get; set; }
        public List<ResourceResponseModel> AvailableResources { get; set; }
    }
}
