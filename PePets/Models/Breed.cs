using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class Breed
    {
        public Guid Id { get; set; }
        public string Dogs { get; set; }
        public string Cats { get; set; }
        public string Birds { get; set; }
        public string Rodents { get; set; }
        public string Fishes { get; set; }
        public string FarmAnimals { get; set; }
        public string Other { get; set; }
    }
}
