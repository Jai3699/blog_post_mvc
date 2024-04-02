using System.ComponentModel.DataAnnotations;

namespace Blogpost.Models.ViewModels
{
    public class AddTagRequest
    {
        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
