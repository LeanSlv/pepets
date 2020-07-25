using System;

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
