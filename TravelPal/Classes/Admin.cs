using TravelPal.Enums;

namespace TravelPal.Classes
{
    class Admin : IUser
    {
        public string Username { get; set; } = "admin";
        public string Password { get; set; } = "password";
        public string FirstName { get; set; } = "Admin";
        public string LastName { get; set; } = "Adminsson";
        public string FullName { get { return FirstName + " " + LastName; } }
        public Countries Location { get; set; }

    }
}
