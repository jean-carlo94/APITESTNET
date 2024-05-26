namespace APITEST.Modules.Products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public int CurrentStock { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
