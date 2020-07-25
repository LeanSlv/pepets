using Microsoft.EntityFrameworkCore;
using PePets.Data;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public class BreedRepository : IBreedRepository
    {
        private readonly PePetsDbContext _context;
        private bool disposed;

        public BreedRepository(PePetsDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(BreedOfPet breed)
        {
            await SaveStateOfBreedAsync(breed, EntityState.Added);
        }

        public async Task UpdateAsync(BreedOfPet breed)
        {
            await SaveStateOfBreedAsync(breed, EntityState.Modified);
        }

        public async Task DeleteAsync(BreedOfPet breed)
        {
            _context.Remove(breed);
            await _context.SaveChangesAsync();
        }

        public async Task<BreedOfPet> GetFirstBreed()
        {
            return await _context.BreedsOfPet.FirstAsync();
        }

        public IEnumerable<BreedOfPet> GetAll()
        {
            return _context.BreedsOfPet;
        }

        public async Task<BreedOfPet> GetByIdAsync(Guid id)
        {
            return await _context.BreedsOfPet.SingleOrDefaultAsync(sod => sod.Id == id);
        }

        public async Task<BreedOfPet> GetByNameAsync(string name)
        {
            return await _context.BreedsOfPet.SingleOrDefaultAsync(sod => sod.Breed == name);
        }

        public async Task<IEnumerable<BreedOfPet>> GetAllBreedsOfTypeAsync(string typeName)
        {
            TypeOfPet type = await _context.TypesOfPet.FirstOrDefaultAsync(fod => fod.Type == typeName);
            return _context.BreedsOfPet.Where(w => w.TypeId == type.Id);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task SaveStateOfBreedAsync(BreedOfPet type, EntityState state)
        {
            _context.Entry(type).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
