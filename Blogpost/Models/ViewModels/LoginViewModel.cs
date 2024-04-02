using System.ComponentModel.DataAnnotations;

namespace Blogpost.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        //for server side validation 
        public string Username {  get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Please enter atleast 6 characters")]
        public string Password { get; set; }
        
    }
}
