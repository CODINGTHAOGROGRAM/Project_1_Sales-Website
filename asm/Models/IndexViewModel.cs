using System.ComponentModel.DataAnnotations.Schema;

namespace asm.Models
{
    [NotMapped]
    public class IndexViewModel
    {
        public List<FoodItem> FoodItems { get; set; }
    }
}
