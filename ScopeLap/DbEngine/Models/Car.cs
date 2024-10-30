using System.ComponentModel.DataAnnotations;

namespace ScopeLap.Models.DataBaseEngine
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо указать марку автомобиля")]
        [MaxLength(20), MinLength(3)]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Необходимо указать модель автомобиля")]
        [MaxLength(20)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Необходимо указать основную информацию об автомобиле")]
        public string Description { get; set; }

        public bool Modified { get; set; } = false;

        public string? ModDescription { get; set; }

        public List<LapSession> Sessions { get; } = [];


    }
}
