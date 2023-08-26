using Carco.Entities;

namespace Carco.Models
{
    public class AddCarAdViewModel
    {
        public string? AdTitle { get; set; }

        public string? CarVIN { get; set; }

        public int CarYear { get; set; }

        public int CarPrice { get; set; }

        public string? CarMake { get; set; }

        public string? CarModel { get; set; }

        public string? CarType { get; set; }

        public string? CarPower { get; set; }

        public int CarKilometers { get; set; }

        public string? CarTransmission { get; set; }

        public string? AdDescription { get; set; }

        public List<IFormFile>? Pictures { get; set; }

    }
}
