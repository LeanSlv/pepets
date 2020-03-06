using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public enum TypeofPet
    {
        Cat,
        Dog
    }

    public class Pet
    {
        public string Sex { get; set; }
        public TypeofPet Type { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Color { get; set; }

    }
}
