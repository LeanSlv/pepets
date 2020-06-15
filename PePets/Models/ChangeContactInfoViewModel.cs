using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Models
{
    public class ChangeContactInfoViewModel
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Genders Gender { get; set; }
        public string AboutMe { get; set; }
    }
}
