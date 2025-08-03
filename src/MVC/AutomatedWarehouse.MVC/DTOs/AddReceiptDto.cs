namespace AutomatedWarehouse.MVC.DTOs
{
    public class AddReceiptDto
    {
        public uint Number { get; set; }
        public DateOnly Date { get; set; }
        public List<AddReceiptReceiptResourceDto> ReceiptResources { get; set; } = new();
    }
}
