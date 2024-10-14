using ScopeLap.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models.DataBaseEngine
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public DateTime Posted { get; set; }

        [Required]
        [MaxLength(255)]
        public string PostText { get; set; }

        public string Content { get; set; }

        public int AccountID { get; set; }

        public Account Account { get; set; } = null!;

        public List<Commentary> Commentaries { get; } = [];

    }
}
