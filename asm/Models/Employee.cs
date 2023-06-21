using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace asm.Models
{
    public enum Position
    {
        [Display(Name = "Manager")]
        Manager = 1,
        [Display(Name = "Employee")]
        Employee = 2
    }
    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 1,
        [Display(Name = "Female")]
        Female = 2,
    }
    public class Employee
    {
        [Key]
        [Column("Id")]
        public int EmployeeID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "User name")]
        [Required(ErrorMessage = "Please enter user name.")]
        [RegularExpression(@"^[a-z][a-z0-9]+$", ErrorMessage = "The username must begin with lowercase only and <br> does not contain special characters and spaces.")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Please enter full name.")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter email.")]
        [RegularExpression("^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$",
            ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please choose position!"),
            Range(1, 2, ErrorMessage = "Invalid position!")]
        [Display(Name = "Position")]
        public Position Position { get; set; }

        [Required(ErrorMessage = "Please choose gender!"),
            Range(1, 2, ErrorMessage = "Invalid gender!")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Please choose the date of birth!")]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [Display(Name = "Phone number")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Locked")]
        public bool Locked { get; set; }

        [Required(ErrorMessage = "Please enter password!")]
        [Column(TypeName = "varchar(50)"), MaxLength(50), MinLength(8, ErrorMessage = "This password too short")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(100)]
        [Display(Name = "Image")]
        public string? Image { get; set; }

        [Column(TypeName = "nvarchar(255)"), MaxLength(50), AllowNull]
        [Display(Name = "Note")]
        public string? Note { get; set; }
        [NotMapped]
        [Display(Name = "Image File")]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not match!")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }


    }
}
