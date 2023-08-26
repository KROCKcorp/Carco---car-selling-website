using Carco.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carco.Data
{
    public class ApplicationUser: IdentityUser
    {

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }
        
        [MaxLength(600)]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime UserBirthDate { get; set; }

        public virtual ICollection<CarAdEntity>? CarAds { get; set; }

    }



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CarAdEntity> CarAds { get; set; }
        public DbSet<CarPictureEntity> CarPictures { get; set; }
        public DbSet<UserFavouriteEntity> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CarAdEntity>()
                .HasOne(c => c.User)
                .WithMany(u => u.CarAds)
                .HasForeignKey(c => c.UserId);

            builder.Entity<CarPictureEntity>()
                .HasOne(p => p.CarAd)
                .WithMany(c => c.Pictures)
                .HasForeignKey(p => p.CarAdId);

            builder.Entity<UserFavouriteEntity>()
            .HasKey(f => f.Id);

            builder.Entity<UserFavouriteEntity>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFavouriteEntity>()
                .HasOne(f => f.CarAd)
                .WithMany()
                .HasForeignKey(f => f.CarAdId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}