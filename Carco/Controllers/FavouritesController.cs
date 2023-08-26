using Carco.Data;
using Carco.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Carco.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly ApplicationDbContext carcoDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouritesController(ApplicationDbContext carcoDbContext, UserManager<ApplicationUser> userManager)
        {
            this.carcoDbContext = carcoDbContext;
            _userManager = userManager;
        }

        //takes car ad id with the current user id and adds it to the favourites table if it doesnt exist allready
        public async Task<IActionResult> Add(int carAdId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);

            var isFavourite = carcoDbContext.Favorites.Any(f => f.CarAdId == carAdId && f.UserId == user.Id);
            if (!isFavourite)
            {
                var favorite = new UserFavouriteEntity
                {
                    UserId = userId,
                    CarAdId = carAdId
                };

                carcoDbContext.Favorites.Add(favorite);
                await carcoDbContext.SaveChangesAsync();

            }
            return RedirectToAction("UserFavouritesView", "Favourites");
        }

        //returns to the user favourites view all the favourite ads related to the logged in user id from the favourites table
        [HttpGet]
        public async Task<IActionResult> UserFavouritesView()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await carcoDbContext.Favorites
                .Include(f => f.CarAd)
                .Where(f => f.UserId == userId)
                .ToListAsync();

            return View(favorites);
        }

        //takes favourite id and removes it from the favourites table if it exists
        public async Task<IActionResult> Remove(int id)
        {
            var favorite = await carcoDbContext.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            carcoDbContext.Favorites.Remove(favorite);
            await carcoDbContext.SaveChangesAsync();

            return RedirectToAction("UserFavouritesView", "Favourites");
        }
    }
}
