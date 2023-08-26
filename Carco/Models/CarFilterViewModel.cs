using Carco.Entities;

namespace Carco.Models
{
    public class CarFilterViewModel
    {
        public List<CarAdEntity> CarAds { get; set; }

        public List<string> Makes { get; set; }

        public List<string> Models { get; set; }

        public List<int> Years { get; set; }

        public List<string> Transmissions { get; set; }

        public List<UserFavouriteEntity> UserFavorites { get; set; }
    }
}
