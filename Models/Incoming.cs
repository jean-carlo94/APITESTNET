using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITEST.Models
{
    public class Incoming
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime IncomingDate { get; set; }

        [Required]
        public string Supplier { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
