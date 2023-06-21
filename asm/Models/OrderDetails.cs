using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace asm.Models
{
    public class OrderDetails
    {
        [Key]
        [Column("Id")]
        public int OrderDetailId { get; set; }

        [ForeignKey("Order")]
        public string OrderId { get; set; }
        [ForeignKey("Menu")]
        public int ProductId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = ("You need to enter the quantity"))]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = ("You need to enter the total"))]
        [Display(Name = "Total")]
        public double Total { get; set; }

        [Display(Name = "Note")]
        public string? Note { get; set; }

        public Order Order { get; set; }
        public FoodItem foodItem { get; set; }
    }
}
