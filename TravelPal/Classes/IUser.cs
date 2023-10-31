using TravelPal.Enums;

namespace TravelPal.Classes
{
    public interface IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public Countries Location { get; set; }
    }
}
