namespace InventoryService.Domain.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public string Status { get; set; }
    }
}
