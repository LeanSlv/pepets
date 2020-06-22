using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public Genders Gender { get; set; }
        public string Location { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AboutMe { get; set; }
        public double Rating { get; set; }
        public List<User> AlreadyRatedUsers { get; set; }
        public List<Advert> Adverts { get; set; }
        public List<Advert> FavoriteAdverts { get; set; }

        public User()
        {
            AlreadyRatedUsers = new List<User>();
            Adverts = new List<Advert>();
            FavoriteAdverts = new List<Advert>();
        }
    }
}
