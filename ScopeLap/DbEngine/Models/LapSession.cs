using ScopeLap.DataBaseEngine;

namespace ScopeLap.Models.DataBaseEngine
{
    public class LapSession
    {
        public int Id { get; set; }

        public int LapTime { get; set; }

        public string? LapNote { get; set; }

        public int? AccountId { get; set; }

        public Account? Account { get; set; } = null;

        public int CarId { get; set; }

        public Car Car { get; set; } = null;

        public List<TrackConfiguration> Tracks { get; } = [];

    }
}
