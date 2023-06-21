using System.ComponentModel.DataAnnotations;

namespace asm.ViewModels
{
    public class ViewWebLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
