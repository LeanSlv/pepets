using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class BreedOfPet
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public TypeOfPet Type { get; set; }
        public string Breed { get; set; }
    }
}
