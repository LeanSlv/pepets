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

        public DbSet<Advert> Adverts { get; set; }
    }
}
