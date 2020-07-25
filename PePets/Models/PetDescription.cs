using System;
using System.ComponentModel.DataAnnotations;

namespace PePets.Models
{
    public enum Genders
    {
        NotChosen,
        Female,
        Male
    }

    public class PetDescription
    {
        [Required]
        public Guid Id { get; set; }
        public Genders Gender { get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Color { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
