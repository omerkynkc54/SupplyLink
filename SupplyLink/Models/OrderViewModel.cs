namespace SupplyLink.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public string? ImagePath { get; set; }
        public string Notes { get; set; }
        public DateTime OrderDate { get; set; }
        public string Type { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Status { get; set; }
        public string RequesterName { get; set; }
    }
}
