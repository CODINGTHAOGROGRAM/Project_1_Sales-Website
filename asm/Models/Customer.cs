using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace asm.Models
{
    public class Customer
    {
        [Key]
        [Column("Id")]
        public int CustomerID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Please enter full name.")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Please choose gender!"),
            Range(1, 2, ErrorMessage = "Invalid gender!")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter email.")]
        [RegularExpression("^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$",
            ErrorMessage = "Email is invalid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please choose the date of birth!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^(\\+?84|0)(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9])[0-9]{7}$",
            ErrorMessage = "Phone number is invalid")]
        [Column(TypeName = "varchar(15)"), MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter address!")]
        [Display(Name = "Address")]
        [MaxLength(250)]
        public string Address { get; set; }
        [Display(Name = "Locked")]
        public bool Locked { get; set; }

        [Required(ErrorMessage = "Please enter password!")]
        [Column(TypeName = "varchar(50)"), MaxLength(50), MinLength(8, ErrorMessage = "This password too short")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(255)"), MaxLength(50)]
        [Display(Name = "Note")]
        public string? Note { get; set; }

        [StringLength(100)]
        [Display(Name = "Image")]
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not match!")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
