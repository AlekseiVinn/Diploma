using ScopeLap.DataBaseEngine;

namespace ScopeLap.Models.DataBaseEngine
{
    public class Post
    {
        public int Id { get; set; }

        public DateTime Posted { get; set; }

        public string PostText { get; set; }

        public string Content { get; set; }

        public int AccountID { get; set; }

        public Account Account { get; set; } = null!;

        public List<Commentary> Commentaries { get; } = [];

    }
}
