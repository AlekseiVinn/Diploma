namespace ScopeLap.Models.DataBaseEngine
{
    public class TrackDay
    {
        public int Id { get; set; }

        public DateOnly TrackDate { get; set; }

        public int LapSessionId { get; set; }

        public int TrackConfigId { get; set; }
    }
}
