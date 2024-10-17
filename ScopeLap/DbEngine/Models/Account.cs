using Microsoft.EntityFrameworkCore;
using ScopeLap.DbEngine;
using ScopeLap.Models.DataBaseEngine;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScopeLap.DataBaseEngine
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HashPass { get; set; }

        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username required.")]
        [MaxLength(20, ErrorMessage = "Must be 20 characters or less"), MinLength(4)]

        public string Username { get; set; }

        [Required(ErrorMessage = "Firstname required.")]
        [MaxLength(20, ErrorMessage = "Name must be 20 characters or less"), MinLength(2)]
        public string Firstname { get; set; }

        [MaxLength(30, ErrorMessage = "Name must be 20 characters or less"), MinLength(6)]
        public string? Lastname { get; set; }

        public ICollection<Post> Posts { get; } = new List<Post>();

        public List<Commentary> Commentaries { get; } = [];

        public List<LapSession> Sessions { get; } = []; 
    }
}
