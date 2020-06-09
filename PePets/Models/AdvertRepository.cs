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
            return _context.Adverts.Include(i => i.PetDescription).Include(i => i.User);
        }

        public Advert GetAdvertById(Guid id)
        {
            return _context.Adverts.Include(p => p.PetDescription).Include(u => u.User).Single(x => x.Id == id);
        }

        public Guid SaveAdvert(Advert entity)
        {       
            if (entity.Id == default)
            {
                // Если объявления не существует, то добавляем её
                _context.Entry(entity).State = EntityState.Added;
                _context.Entry(entity.PetDescription).State = EntityState.Added;
            } 
            else
            {
                // Иначе обновляем        
                _context.Entry(entity).State = EntityState.Modified;
                _context.Entry(entity.PetDescription).State = EntityState.Modified;
            }

            _context.SaveChanges();

            return entity.Id;
        }

        public void DeleteAdvert(Advert entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void LikeAdvert(Advert advert)
        {
            advert.NumberOfLikes += 1;
            _context.Entry(advert).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UnlikeAdvert(Advert advert)
        {
            advert.NumberOfLikes -= 1;
            _context.Entry(advert).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void addViewToAdvert(Advert advert)
        {
            advert.Views += 1;
            _context.Entry(advert).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
