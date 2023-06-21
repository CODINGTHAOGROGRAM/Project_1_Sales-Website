using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace asm.Models
{
    public enum TypeFood
    {
        [Display(Name ="Món")]
        Dish = 1,
        [Display(Name ="Combo")]
        Combo = 2,
        [Display(Name = "Nước")]
        WaterDish = 3
    }
    public class FoodItem
    {
        [Key]
        public int FoodId { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage ="Bận cần nhập tên.")]
        [Display(Name ="Tên")]
        [Column(TypeName ="nvarchar(200)")]
        public string FoodName { get; set; }

        [StringLength (250)]
        [Display(Name ="Mô tả")]
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Bạn cần nhập giá."),Range(0,double.MaxValue,ErrorMessage ="Bạn cần nhập giá.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name ="Giá")]
        public decimal Price { get; set; }

        [Display(Name ="Phân loại")]
        [Required(ErrorMessage ="Bạn cần chọn phân loại"),Range(1,int.MaxValue,ErrorMessage ="Bạn cần chọn phân loại món.")]
        public TypeFood KindFood { get; set; }

        [StringLength(100)]
        [Display(Name ="Hình")]
        public string Img { get; set; }
        [NotMapped]
        [Display(Name ="Chọn hình")]
        public IFormFile ImageFile { get; set; }

        [Display(Name ="Đang phục vụ")]
        public bool Status { get; set; }

    }
}
