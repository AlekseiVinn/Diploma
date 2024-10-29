namespace ScopeLap.Models
{
    public class ListSessionViewModel
    {

        public int? Id { get; set; }

        public int? Time { get; set; }

        public String PrintTime { get; set; }

        public int CarID { get; set; }

        public String CarName { get; set; }

        public int UserId { get; set; }

        public String Username { get; set; }

        public int TrackId { get; set; }

        public int TrackConfigId { get; set; }

        public String TrackName { get; set; }

        public DateOnly SessionDate { get; set; }

        public String? SessionNote { get; set; }

    }

}
