using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using asm.Models;

namespace asm.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Order received")]
        Received = 1,
        [Display(Name = "Order processing")]
        Processing = 2,
        [Display(Name = "Delivering")]
        Delivering = 3,
        [Display(Name = "Delivered")]
        Delivered = 4,
        [Display(Name = "Cancelled")]
        Cancelled = 5
    }
    public class Order
    {
        [Key]
        [Column("Id")]
        public string OrderId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "You need to select an order date")]
        [Display(Name = "Order date")]

        public DateTime OrderDate { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "You need to enter a price")]
        [Display(Name = "Total")]
        public double Total { get; set; }
        [Required(ErrorMessage = "You need to select an order status")]
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }
        [Display(Name = "Method")]
        public string Method { get; set; }
        [StringLength(250)]
        [Display(Name = "Note")]
        public string? Note { get; set; }

        [Display(Name = "Delete")]
        public bool Delete { get; set; }

        public bool PointAdded { get; set; }

        public Customer? Customer { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }

    }
}
