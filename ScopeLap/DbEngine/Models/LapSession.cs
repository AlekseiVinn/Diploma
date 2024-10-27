using ScopeLap.DataBaseEngine;
using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models.DataBaseEngine
{
    public class LapSession
    {
        [Key]
        public int Id { get; set; }

        public int LapTime { get; set; }

        public string? LapNote { get; set; }

        public DateOnly TrackDate { get; set; }

        public int? AccountId { get; set; }

        public Account? Account { get; set; } = null;

        public int CarId { get; set; }

        public Car Car { get; set; } = null;

        public int? TrackId { get; set; }

        public TrackConfiguration? Track { get; set;}

    }
}
