using Carco.Data;

namespace Carco.Entities
{
    public class UserFavouriteEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CarAdId { get; set; }
        public CarAdEntity CarAd { get; set; }
    }
}
