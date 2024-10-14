namespace ScopeLap.Models.DataBaseEngine
{
    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<TrackConfiguration> TrackConfigurations { get; } = [];

    }
}
