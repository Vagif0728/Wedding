using System.ComponentModel.DataAnnotations;

namespace SweetWeeding.Areas.Admin.ViewModels
{
    public class UpdateFamilyVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }
        
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
