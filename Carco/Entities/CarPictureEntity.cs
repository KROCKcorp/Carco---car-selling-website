using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carco.Entities
{
    public class CarPictureEntity
    {
        public int Id { get; set; }

        [Required]
        public byte[]? CarPhoto { get; set; }

        public int CarAdId { get; set; }

        public CarAdEntity? CarAd { get; set; }
    }
}
