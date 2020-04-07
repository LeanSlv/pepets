using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public enum Genders
    {
        NotChosen,
        Female,
        Male
    }
    public enum TypeofPet
    {
        NotChosen,
        Cat,
        Dog
    }

    public class PetDescription
    {
        [Required]
        public Guid Id { get; set; }
        public Genders Sex { get; set; }
        public TypeofPet Type { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Color { get; set; }
        public Guid AdvertId { get; set; }
        public Advert Advert { get; set; }
    }
}
