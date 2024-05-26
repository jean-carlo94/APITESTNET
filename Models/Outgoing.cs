using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APITEST.Models
{
    public class Outgoing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime OutgoingDate { get; set; }

        [Required]
        public string Customer { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
