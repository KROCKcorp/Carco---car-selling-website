using Carco.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carco.Entities
{
    public class CarAdEntity
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string? AdTitle { get; set; }

        [MaxLength(17)]
        public string? CarVIN { get; set; }

        [MaxLength(4)]
        public int CarYear { get; set; }

        [MaxLength(50)]
        public string? CarMake { get; set; }

        [MaxLength(100)]
        public string? CarType { get; set; }

        [MaxLength(100)]
        public string? CarPower { get; set; }

        [MaxLength(100)]
        public string? CarModel { get; set; }

        [MaxLength(9)]
        public int CarKilometers { get; set; }

        [MaxLength(15)]
        public int CarPrice { get; set; }

        public string? CarTransmission { get; set; }

        [MaxLength(4000)]
        public string? AdDescription { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<CarPictureEntity> Pictures { get; set; }

        public ICollection<UserFavouriteEntity> Favorites { get; set; }

    }
}
