using ScopeLap.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models.DataBaseEngine
{
    public class Commentary
    {
        [Key]
        public int Id { get; set; }

        public DateTime Commented { get; set; }

        [Required]
        [MaxLength(255)]
        public string CommentText { get; set; }

        public int? AccountID { get; set; }

        public Account? Account { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

    }
}
