using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class PePetsDbContext : DbContext
    {
        public PePetsDbContext(DbContextOptions<PePetsDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advert>()
                .Property(t => t._images).HasColumnName("Images");
        }

        public DbSet<Advert> Adverts { get; set; }
        public DbSet<PetDescription> PetsDescription { get; set; }
    }
}
