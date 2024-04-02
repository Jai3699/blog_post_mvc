using System.ComponentModel.DataAnnotations;

namespace Blogpost.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username {  get; set; }
        [Required]
        [EmailAddress]
        public string Email {  get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Please enter atleast 6 digits password")]
        public string Password { get; set; }    
    }
}
