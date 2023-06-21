using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace asm.Models
{
    public class PaymentResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PaymentId { get; set; }
        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PayerId { get; set; }
        public bool Success { get; set; }
        public Order? Order { get; set; }
    }
}
