using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class Advert
    {
        internal string _images { get; set; }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public string[] Images { 
            get { return _images == null ? null : JsonConvert.DeserializeObject<string[]>(_images); }
            set { _images = JsonConvert.SerializeObject(value); } 
        }
        public int Cost { get; set; }
        public Pet PetDecription { get; set; }
        public Guid UserId { get; set; }
        public string Location { get; set; }
        public int NumberOfLikes { get; set; }
        public int Views { get; set; }
        public DateTime PublicationDate { get; set; }

    }
}
