using Microsoft.EntityFrameworkCore;
using ScopeLap.DbEngine;
using ScopeLap.Models.DataBaseEngine;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScopeLap.DataBaseEngine
{
    public class Account
    {
        [Required]
        [PasswordPropertyText]
        public string HashPass { get; set; }

        [Required]
        [UniqueMail(ErrorMessage = "Email already taken")]
        [EmailAddress]
        public string Email { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "BloggerName must be 20 characters or less"), MinLength(2)]
        public string Firstname { get; set; }

        public string? Lastname { get; set; }

        public ICollection<Post> Posts { get; } = new List<Post>();

        public List<Commentary> Commentaries { get; } = [];

        public List<LapSession> Sessions { get; } = [];
    }
}
