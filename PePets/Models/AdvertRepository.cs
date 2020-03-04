using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class AdvertRepository
    {
        private readonly PePetsDbContext _context;

        public AdvertRepository(PePetsDbContext context)
        {
            _context = context;
        }

        public IQueryable<Advert> GetAdverts()
        {
            return _context.Adverts.OrderBy(x => x.Title);
        }

        public Advert GetAdvertById(int id)
        {
            return _context.Adverts.Single(x => x.Id == id);
        }

        public int SaveAdvert(Advert entity)
        {
            
            if (entity.Id == default)
                // Если статьи не существует, то добавляем её
                _context.Entry(entity).State = EntityState.Added;
            else
                // Иначе обновляем
                _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

            return entity.Id;
        }

        public void DeleteAdvert(Advert entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}
