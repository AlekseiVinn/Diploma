using ScopeLap.DataBaseEngine;
using ScopeLap.Models.DataBaseEngine;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ScopeLap.Models
{
    [Serializable]
    public class SessionViewModel
    {

        public int? Id { get; set; }

        [Required(ErrorMessage = "Укажите время")]
        [Range(0,60)]
        public int Minutes { get; set; }

        [Required(ErrorMessage = "Укажите время")]
        [Range(0, 59)]
        public int Seconds { get; set; }

        [Required(ErrorMessage = "Укажите время")]
        [Range(0, 999)]
        public int Miliseconds { get; set; }

        [Required(ErrorMessage = "Укажите дату заезда")]
        public DateOnly SessionDate { get; set; }

        public string? LapNote { get; set; }

        public int? AccountId { get; set; }

        public int CarID { get; set; }

        public int TrackConfId { get; set; }
    }
}
