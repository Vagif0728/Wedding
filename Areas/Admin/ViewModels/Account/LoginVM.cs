using System.ComponentModel.DataAnnotations;

namespace SweetWeeding.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(5)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRememberMe { get; set; }
    }
}
