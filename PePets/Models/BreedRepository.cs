using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class BreedRepository
    {
        private readonly PePetsDbContext _context;
        public BreedRepository(PePetsDbContext context)
        {
            _context = context;
        }

        public IQueryable<BreedOfPet> Breeds
        {
            get { return _context.BreedsOfPet; }
        }

        public BreedOfPet GetFirstBreed()
        {
            var breed = _context.BreedsOfPet.First();
            return breed;
        }

        public IQueryable<BreedOfPet> GetAllBreedsOfType(string typeName)
        {
            var type = _context.TypesOfPet.Single(x => x.Type == typeName);
            var breeds = _context.BreedsOfPet.Where(x => x.TypeId == type.Id);
            return breeds;
        }

        public BreedOfPet FindBreedById(Guid id)
        {
            return _context.BreedsOfPet.SingleOrDefault(x => x.Id == id);
        }

        public bool SaveBreed(BreedOfPet breed)
        {
            if (breed.Id == default)
            {
                // Если статьи не существует, то добавляем её
                _context.Entry(breed).State = EntityState.Added;
            }
            else
            {
                // Иначе обновляем        
                _context.Entry(breed).State = EntityState.Modified;
            }

            _context.SaveChanges();

            return true;
        }

        public void DeleteBreed(BreedOfPet breed)
        {
            _context.Remove(breed);
            _context.SaveChanges();
        }
    }
}
