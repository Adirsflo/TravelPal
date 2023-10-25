using TravelPal.Enums;

namespace TravelPal.Classes
{
    public class User : IUser
    {


        public string Username { get; set; }
        public string Password { get; set; }
        public Country Location { get; set; }

        public User(string username, string password, Country location)
        {
            Username = username;
            Password = password;
            Location = location;
        }
    }
}
