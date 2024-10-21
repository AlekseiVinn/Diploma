using ScopeLap.DataBaseEngine;
using ScopeLap.Models.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models
{
    public class SessionViewModel
    {

        public int Id { get; set; }

        public int LapTime { get; set; }

        public string? LapNote { get; set; }

        public int? AccountId { get; set; }

        public Car Car { get; set; } = null;

        public List<TrackConfiguration> Tracks { get; } = [];
    }
}
