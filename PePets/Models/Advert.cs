using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class Advert
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Image> Images { get; set; }
        public int Cost { get; set; }
        public Pet PetDecription { get; set; }
        public Guid UserId { get; set; }
        public string Location { get; set; }
        public int NumberOfLikes { get; set; }
        public int Views { get; set; }
        public DateTime PublicationDate { get; set; }

    }
}
