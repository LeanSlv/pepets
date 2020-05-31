using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class TypeRepository
    {
        private readonly PePetsDbContext _context;

        public TypeRepository(PePetsDbContext context)
        {
            _context = context;
        }

        public IQueryable<TypeOfPet> Types { get { return _context.TypesOfPet; } }

        public bool SaveType(TypeOfPet type)
        {
            if (type.Id == default)
            {
                // Если статьи не существует, то добавляем её
                _context.Entry(type).State = EntityState.Added;
            }
            else
            {
                // Иначе обновляем        
                _context.Entry(type).State = EntityState.Modified;
            }

            _context.SaveChanges();

            return true;
        }

        public TypeOfPet FindTypeById(Guid id) => _context.TypesOfPet.SingleOrDefault(x => x.Id == id);

        public TypeOfPet FindTypeByName(string typeName) => 
            _context.TypesOfPet.SingleOrDefault(x => x.Type == typeName);

        public void DeleteType(TypeOfPet type)
        {
            _context.Remove(type);
            _context.SaveChanges();
        }
    }
}
