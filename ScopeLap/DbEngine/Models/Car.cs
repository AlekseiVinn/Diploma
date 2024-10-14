namespace ScopeLap.Models.DataBaseEngine
{
    public class Car
    {
        public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public bool Modified { get; set; } = false;

        public string ModDescription { get; set; }

        public List<LapSession> Sessions { get; } = [];


    }
}
