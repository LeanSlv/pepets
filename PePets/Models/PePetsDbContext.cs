using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PePets.Models
{
    public class PePetsDbContext : IdentityDbContext<User>
    {
        public PePetsDbContext(DbContextOptions<PePetsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Advert>().
                Property(t => t._images).HasColumnName("Images");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Adverts)
                .WithOne(a => a.User).HasForeignKey(a => a.UserId);
        }

        public DbSet<Advert> Adverts { get; set; }
        public DbSet<PetDescription> PetsDescription { get; set; }
    }
}
