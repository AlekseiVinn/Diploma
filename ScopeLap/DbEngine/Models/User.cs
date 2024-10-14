using ScopeLap.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models.DataBaseEngine
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string HashPass { get; set; }

        [Required]
        public string Email { get; set; }

        public Account? UserAccount { get; set; }
    }
}
