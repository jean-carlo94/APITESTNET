using System.ComponentModel.DataAnnotations.Schema;

namespace APITEST.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public int Stok { get; set; }
        public int ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
