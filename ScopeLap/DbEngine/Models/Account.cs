using ScopeLap.Models.DataBaseEngine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScopeLap.DataBaseEngine
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string? Lastname { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Post> Posts { get; } = new List<Post>();

        public List<Commentary> Commentaries { get; } = [];

        public List<LapSession> Sessions { get; } = [];
    }
}
