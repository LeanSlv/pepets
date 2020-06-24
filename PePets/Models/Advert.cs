using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PePets.Models
{
    public class Advert
    {
        internal string _images { get; set; }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public string[] Images { 
            get { return _images == null ? null : JsonConvert.DeserializeObject<string[]>(_images); }
            set { _images = JsonConvert.SerializeObject(value); } 
        }
        public int Cost { get; set; }
        public string PhoneNumber { get; set; }
        public PetDescription PetDescription { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string? UserFavoritesId { get; set; }
        public User UserFavorites { get; set; }
        public string Location { get; set; }
        public int NumberOfLikes { get; set; }
        public int Views { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
