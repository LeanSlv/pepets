using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public Genders Gender { get; set; }
        public string Location { get; set; }
        public string Avatar { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Advert> Adverts { get; set; }
        public List<Advert> FavoriteAdverts { get; set; }
    }
}
