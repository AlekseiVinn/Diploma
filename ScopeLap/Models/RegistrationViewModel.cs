using ScopeLap.Models.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20), MinLength(6)]
        public string HashPass { get; set; }

        [Compare("HashPass", ErrorMessage = "Password and Check password must be the same!")]
        [DataType(DataType.Password)]
        [MaxLength(20), MinLength(6)]

        public string HashCheck { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username required.")]
        [MaxLength(20, ErrorMessage = "Must be 20 characters or less"), MinLength(4)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name must be 20 characters or less"), MinLength(2)]
        public string Firstname { get; set; }

        [MaxLength(30, ErrorMessage = "Name must be 20 characters or less"), MinLength(6)]
        public string? Lastname { get; set; }
    }
}
