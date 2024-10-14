using ScopeLap.DataBaseEngine;

namespace ScopeLap.Models.DataBaseEngine
{
    public class Commentary
    {
        public int Id { get; set; }

        public DateTime Commented { get; set; }

        public string CommentText { get; set; }

        public int? AccountID { get; set; }

        public Account? Account { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

    }
}
