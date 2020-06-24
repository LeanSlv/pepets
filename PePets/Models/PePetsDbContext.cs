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

            modelBuilder.Entity<Post>().
                Property(t => t._images).HasColumnName("Images");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(a => a.User).HasForeignKey(a => a.UserId);
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PetDescription> PetsDescription { get; set; }
        public virtual DbSet<TypeOfPet> TypesOfPet { get; set; }
        public virtual DbSet<BreedOfPet> BreedsOfPet { get; set; }
    }
}
