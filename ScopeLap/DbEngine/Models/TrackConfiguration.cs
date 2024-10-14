using System.Security.Cryptography.X509Certificates;

namespace ScopeLap.Models.DataBaseEngine
{
    public class TrackConfiguration
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Length { get; set; }

        public int TrackId { get; set; }

        public Track Track { get; set; } = null!;

        public List<LapSession> Sessions { get; } = [];
    }
}
