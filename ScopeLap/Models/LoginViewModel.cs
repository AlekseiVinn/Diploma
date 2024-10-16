using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20), MinLength(6)]
        public string HashPass { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }
    }
}
