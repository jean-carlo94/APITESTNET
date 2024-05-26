using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITEST.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Barcode { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CurrentStock { get; set; }
        public int ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategories { get; set; }

        public ICollection<Incoming> Incomings { get; set; }
        public ICollection<Outgoing> Outgoings { get; set; }
    }
}
