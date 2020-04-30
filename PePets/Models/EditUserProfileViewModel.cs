namespace PePets.Models
{
    public class EditUserProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Location { get; set; }
        public Genders Gender { get; set; }
        public string Avatar { get; set; }
    }
}
