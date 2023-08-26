using Carco.Data;
using Carco.Entities;
using Carco.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Carco.Controllers
{
    public class CarAdController : Controller
    {
        private readonly ApplicationDbContext carcoDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarAdController(ApplicationDbContext carcoDbContext, UserManager<ApplicationUser> userManager)
        {
            this.carcoDbContext = carcoDbContext;
            _userManager = userManager;
        }

        //returns the sell page form view
        [HttpGet,Authorize]
        public IActionResult Sell()
        {
            return View();
        }

        //returns the sell done confirmation view
        [Authorize]
        public IActionResult SellDone()
        {
            return View();
        }

        //gets the sell form data and saves it to the database
        [HttpPost,Authorize]
        public async Task<IActionResult> Sell(AddCarAdViewModel addCarAddRequest, List<IFormFile> pictures, 
            string selectedTransmission, string selectedBodyType, string selectedFuelType)
        {
            var user = await _userManager.GetUserAsync(User);

            if (pictures.Count > 14 || pictures == null)
            {
                ModelState.AddModelError("pictures", "You can only select up to 14 images.");
            }

            else
            {
                var car_ad = new CarAdEntity()
                {
                    AdTitle = addCarAddRequest.AdTitle,
                    CarVIN = addCarAddRequest.CarVIN,
                    CarYear = addCarAddRequest.CarYear,
                    CarMake = addCarAddRequest.CarMake,
                    CarModel = addCarAddRequest.CarModel,
                    CarKilometers = addCarAddRequest.CarKilometers,
                    CarPrice = addCarAddRequest.CarPrice,
                    CarTransmission = selectedTransmission,
                    CarPower = selectedFuelType,
                    CarType = selectedBodyType,
                    AdDescription = addCarAddRequest.AdDescription,
                    Pictures = new List<CarPictureEntity>(),
                    UserId = user.Id
                };

                await carcoDbContext.CarAds.AddAsync(car_ad);
                await carcoDbContext.SaveChangesAsync();

                foreach (var picture in pictures)
                {
                    using var stream = new MemoryStream();
                    await picture.CopyToAsync(stream);
                    var pictureData = stream.ToArray();

                    CarPictureEntity carPicture = new()
                    {
                        CarPhoto = pictureData,
                        CarAdId = car_ad.Id,
                    };

                    await carcoDbContext.CarPictures.AddAsync(carPicture);
                }
                await carcoDbContext.SaveChangesAsync();
                return View("SellDone");
            }
            return View("Sell");
        }

        //gets the filtering and search data from the buy view and retuns model for car ads to be displayed
        [Authorize]
        public async Task<IActionResult> Buy(string make, string model, string year, string transmission, string search)
        {
            var carAds = carcoDbContext.CarAds.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                carAds = carAds.Where(c => c.AdTitle.Contains(search));
            }

            if (!string.IsNullOrEmpty(make))
            {
                carAds = carAds.Where(c => c.CarMake == make);
            }

            if (!string.IsNullOrEmpty(model))
            {
                carAds = carAds.Where(c => c.CarModel == model);
            }
            if (!string.IsNullOrEmpty(transmission))
            {
                carAds = carAds.Where(c => c.CarTransmission == transmission);
            }
            if (!string.IsNullOrEmpty(year))
            {
                carAds = carAds.Where(c => c.CarYear ==int.Parse(year));
            }

            var viewModel = new CarFilterViewModel
            {
                CarAds = await carAds.ToListAsync(),
                Makes = await carcoDbContext.CarAds.Select(c => c.CarMake).Distinct().ToListAsync(),
                Models = await carcoDbContext.CarAds.Select(c => c.CarModel).Distinct().ToListAsync(),
                Transmissions = await carcoDbContext.CarAds.Select(c => c.CarTransmission).Distinct().ToListAsync(),
                Years = await carcoDbContext.CarAds.Select(c => c.CarYear).Distinct().ToListAsync()
            };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                viewModel.UserFavorites = await carcoDbContext.Favorites
                    .Where(f => f.UserId == userId)
                    .ToListAsync();
            }

            return View(viewModel);
        }

        //takes car ad id and returns the first file of an image related to the car ad in the database
        public IActionResult GetImage(int id)
        {
            var carPic = carcoDbContext.CarPictures.FirstOrDefault(cp => cp.CarAdId == id);
            if (carPic != null)
            {
                return File(carPic.CarPhoto,"image/jpeg");
            }
            return NotFound();
        }

        //takes picture id and the image file
        public IActionResult GetAllImages(int id)
        {
            var carPic = carcoDbContext.CarPictures.Find(id);
            if (carPic != null)
            {
                return File(carPic.CarPhoto,"image/jpeg");
            }
            return NotFound();
        }

        //takes car ad id and returns the car view with the related car ad entity to view car data in the car view
        public async Task<IActionResult> Car(int id)
        {
            var carAd = await carcoDbContext.CarAds.Include(c => c.User).Include(c => c.Pictures)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carAd == null)
            {
                return NotFound();
            }

            return View(carAd);
        }

        //takes car ad id and deletes the corresponding ad from the carAds table and the favourite table
        public async Task<IActionResult> RemoveAd(int id)
        {
            var carAd = await carcoDbContext.CarAds.FindAsync(id);

            if (carAd == null)
            {
                return NotFound();
            }

            var favorites = carcoDbContext.Favorites.Where(f => f.CarAdId == id);
            carcoDbContext.Favorites.RemoveRange(favorites);

            carcoDbContext.CarAds.Remove(carAd);
            await carcoDbContext.SaveChangesAsync();

            return RedirectToAction("UserListings", "CarAd");
        }

        //returns the user listings view with listings list of car ad entities related to the logged in user
        [HttpGet]
        public async Task<IActionResult> UserListings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var listings = await carcoDbContext.CarAds
                .Where(f => f.UserId == userId)
                .ToListAsync();

            return View(listings);
        }
    }
}

