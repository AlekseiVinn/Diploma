using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20), MinLength(6)]
        public string HashPass { get; set; }

        [Required(ErrorMessage = "Username required.")]
        [MaxLength(20, ErrorMessage = "Must be 20 characters or less"), MinLength(4)]
        [DisplayName("Пользователь или электронная почта")]
        public string UsernameOrEmail { get; set; }
    }
}
