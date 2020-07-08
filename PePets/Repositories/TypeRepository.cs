using Microsoft.EntityFrameworkCore;
using PePets.Data;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly PePetsDbContext _context;
        private bool disposed;

        public TypeRepository(PePetsDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TypeOfPet type)
        {
            await SaveStateOfTypeAsync(type, EntityState.Added);
        }

        public async Task UpdateAsync(TypeOfPet type)
        {
            await SaveStateOfTypeAsync(type, EntityState.Modified);
        }

        public async Task DeleteAsync(TypeOfPet type)
        {
            _context.Remove(type);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<TypeOfPet> GetAll()
        {
            return _context.TypesOfPet;
        }

        public async Task<TypeOfPet> GetByIdAsync(Guid id)
        {
            return await _context.TypesOfPet.SingleOrDefaultAsync(sod => sod.Id == id);
        }

        public async Task<TypeOfPet> GetByNameAsync(string typeName)
        {
            return await _context.TypesOfPet.SingleOrDefaultAsync(sod => sod.Type == typeName);
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

        private async Task SaveStateOfTypeAsync(TypeOfPet type, EntityState state)
        {
            _context.Entry(type).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
