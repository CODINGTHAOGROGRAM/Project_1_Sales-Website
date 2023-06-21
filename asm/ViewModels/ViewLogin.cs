using System.ComponentModel.DataAnnotations;

namespace asm.ViewModels
{
    public class ViewLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}
